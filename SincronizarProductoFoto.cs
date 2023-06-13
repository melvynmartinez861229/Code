// histoclin.frmMonitor
using System;
using System.Data;
using System.IO;
using System.Net;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using ADODB;
using histoclin.My;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;

private void SincronizarProductoFoto()
{
	DataTable tbl_rpa_t_producto_foto = await tb_Recordset_MySQL_local("SELECT t_producto.clave, t_producto.nombre, rpa_t_producto_foto.id, rpa_t_producto_foto.id_producto, rpa_t_producto_foto.id_plataforma FROM rpa_t_producto_foto INNER JOIN t_producto ON (t_producto.id_producto = rpa_t_producto_foto.id_producto) WHERE rpa_t_producto_foto.sinc = 1 and rpa_t_producto_foto.id_producto > 0", "rpa_t_producto_foto");
	if (tbl_rpa_t_producto_foto.Rows.Count == 0)
	{
		return;
	}

	int id_plataforma = 0;
	int id_producto = 0;
	string clave = "";
	string nombre = "";

	ProgressBarX1.Minimum = 0;
	ProgressBarX1.Maximum = tbl_rpa_t_producto_foto.Rows.Count;
	checked
	{
		int tbl_rpa_t_producto_foto_Total = tbl_rpa_t_producto_foto.Rows.Count - 1;
		int position = 0;
		while (true)
		{
			if (position > tbl_rpa_t_producto_foto_Total)
			{
				break;
			}
			id_producto = Convert.ToInt32(tbl_rpa_t_producto_foto.Rows[position]["id_producto"].ToString());
			id_plataforma = Convert.ToInt32(tbl_rpa_t_producto_foto.Rows[position]["id_plataforma"].ToString());
			string filename = "";
			clave = tbl_rpa_t_producto_foto.Rows[position]["clave"].ToString();
			nombre = tbl_rpa_t_producto_foto.Rows[position]["nombre"].ToString().Trim();
			nombre = slug_value(nombre);
			PictureBox pictureBox = new PictureBox();
			pictureBox.Image = null;            
			string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
			try
			{
				DataTable tbl_t_producto_fotoID = await tb_Recordset_MySQL_local("SELECT foto FROM t_producto_foto WHERE id_producto = " + Convert.ToString(id_producto) + "", "t_producto_foto");
				if (tbl_t_producto_fotoID.Rows.Count != 0)
				{
					if (String.Compare(tbl_t_producto_fotoID.Rows[0][0].ToString(), "",  false) == 0)
					{
						pictureBox.Image = null;
					}
					else
					{
						Guid clsid = new Guid("00000566-0000-0010-8000-00AA006D2EA4");
						ADODB.Stream stream = (ADODB.Stream)Activator.CreateInstance(Marshal.GetTypeFromCLSID(clsid));
						if (stream.State == ObjectStateEnum.adStateOpen)
						{
							stream.Close();
						}
						stream.Type = StreamTypeEnum.adTypeBinary;
						stream.Open(Missing.Value);
						try
						{
							stream.Write(RuntimeHelpers.GetObjectValue(tbl_t_producto_fotoID.Rows[0][0]));
						}
						catch (Exception ex)
						{
							
						}
						stream.Read();
						stream.SaveToFile(baseDirectory + "\\" + nombre + ".png", SaveOptionsEnum.adSaveCreateOverWrite);
						stream.Close();
					}
					goto IL_02e9;
				}
				await Update_local("UPDATE rpa_t_producto_foto SET updated_at_plat = current_timestamp, sinc = 0 WHERE id = " + tbl_rpa_t_producto_foto.Rows[position]["id"].ToString(), "rpa_t_producto_foto");
			}
			catch (Exception ex3)
			{
				await Log_DB("test 9", ex4.Message + " error al leer foto y el stream id_producto: " + Convert.ToString(id_producto));
			}
			goto IL_0bf4;
			IL_0bf4:
			ProgressBarX1.Value += 1;
			position++;
			continue;

			IL_02e9:			
			string path_FileName = baseDirectory + nombre + ".png";
			filename = Path.GetFileName(path_FileName);
			if (id_plataforma == 0)
			{
				DataTable tbl_products = await tb_Recordset_MySQL_Plataforma("SELECT id from products where sku = '" + clave + "'", "products");
				if (tbl_products.Rows.Count > 0)
				{
					id_plataforma = Convert.ToInt32(tbl_products.Rows[0][0].ToString());
					await Update_local("UPDATE rpa_t_producto_foto SET updated_at_plat = current_timestamp, id_plataforma = " + Convert.ToString(id_plataforma) + " WHERE id = " + tbl_rpa_t_producto_foto.Rows[position]["id"].ToString(), "rpa_t_producto_foto");
				}
				else
				{
					DataTable tbl_rpa_t_producto = tb_Recordset_MySQL_local("Select id_plataforma from rpa_t_producto where id_producto = " + Convert.ToString(id_producto) + "", "rpa_t_producto en img prod");
					if (tbl_rpa_t_producto.Rows.Count == 0)
					{
						await Log_DB("No existe el producto " + Convert.ToString(id_producto) + " para subir imagen", "rpa_t_producto valida idproducto");
						goto IL_0bf4;
					}
					id_plataforma = Convert.ToInt32(tbl_rpa_t_producto.Rows[0]["id_plataforma"].ToString());
					await Update_local("UPDATE rpa_t_producto_foto SET updated_at_plat = current_timestamp, id_plataforma = " + Convert.ToString(id_plataforma) + " WHERE id = " + tbl_rpa_t_producto_foto.Rows[position]["id"].ToString(), "rpa_t_producto_foto");
				}
			}

			try
			{
				ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
				string uriString = PATH_FTP_SERVER + "" + Convert.ToString(id_plataforma) + "/";
				bool flag = false;
				FtpWebRequest ftpWebRequest = (FtpWebRequest)WebRequest.Create(new Uri(uriString));
				ftpWebRequest.Credentials = new NetworkCredential(FTP_USER_PLATAFORMA, FTP_PASSWORD_PLATAFORMA);
				ftpWebRequest.Method = "MKD";

				try
				{
					FtpWebResponse ftpWebResponse = (FtpWebResponse)ftpWebRequest.GetResponse();
					ftpWebResponse.Close();
					flag = false;
				}
				catch (Exception ex5)
				{
					flag = true;
				}

				try
				{
					DataTable products = await tb_Recordset_MySQL_Plataforma("SELECT images from products where id = '" + Convert.ToString(id_plataforma) + "'", "products select");
					if (String.Compare(products.Rows[0][0].ToString(), "no-photo.png",  false) != 0 && flag)
					{
						filename = products.Rows[0][0].ToString();
						string uriString2 = PATH_FTP_SERVER + "/" + Convert.ToString(id_plataforma) + "/" + filename;
						TextBox textBox = new TextBox();
						textBox.Text = filename;
						FtpWebRequest ftpWebRequest2 = (FtpWebRequest)WebRequest.Create(new Uri(uriString2));
						ftpWebRequest2.Credentials = new NetworkCredential(FTP_USER_PLATAFORMA, FTP_PASSWORD_PLATAFORMA);
						ftpWebRequest2.Method = "DELE";
						FtpWebResponse ftpWebResponse2 = (FtpWebResponse)ftpWebRequest2.GetResponse();
						ftpWebResponse2.Close();
						textBox.Text = slug_value(textBox.Text);
						int num7 = Strings.Len(textBox.Text) - 4;
						textBox.Select(0, num7);
						string text7 = "";
						text7 = textBox.SelectedText;
						text7 += "-1";
						textBox.Select(num7, 4);
						string text8 = "";
						text8 = textBox.SelectedText;
						filename = text7 + text8;
						nombre = text7;
					}
				}
				catch (Exception ex7)
				{

				}
				MyProject.Computer.Network.UploadFile(path_FileName, PATH_FTP_SERVER + "/" + Convert.ToString(id_plataforma) + "/" + filename, FTP_USER_PLATAFORMA, FTP_PASSWORD_PLATAFORMA);
				await Update_local("UPDATE rpa_t_producto_foto SET updated_at_plat = current_timestamp, sinc = 0 WHERE id = " + tbl_rpa_t_producto_foto.Rows[position]["id"].ToString(), "rpa_t_producto_foto");
			}
			catch (Exception ex9)
			{
				await Log_DB(ex10.Message, "Erro al subir la imagen en la plataforma");
				goto IL_0bf4;
			}
			clave = tbl_rpa_t_producto_foto.Rows[position]["clave"].ToString().Trim();
			if (await Update_Plataforma("UPDATE products SET slug = '" + nombre + "', images = '" + filename + "', cover_image = '" + filename + "' WHERE id = '" + Convert.ToString(id_plataforma) + "'", "products"))
			{
				await Update_local("UPDATE rpa_t_producto_foto SET updated_at_plat = current_timestamp, sinc = 0 WHERE id = " + tbl_rpa_t_producto_foto.Rows[position]["id"].ToString(), "rpa_t_producto_foto");
			}
			goto IL_0bf4;
		}
		ProgressBarX1.Value = 0;
	}
}
