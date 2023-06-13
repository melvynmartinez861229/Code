// histoclin.frmMonitor
using System.Data;
using Microsoft.VisualBasic.CompilerServices;

private void SincronizarCategorias()
{
	DataTable tbl_familia = await tb_Recordset_MySQL_local("SELECT t_familia.clave, t_familia.nombre, t_familia.activo, rpa_t_familia.id, rpa_t_familia.id_familia, rpa_t_familia.id_plataforma FROM rpa_t_familia INNER JOIN t_familia ON (t_familia.id_familia = rpa_t_familia.id_familia) WHERE rpa_t_familia.sinc = 1", "rpa_t_familia INNER t_familia");
	if (tbl_familia.Rows.Count == 0)
	{
		return;
	}
	int id_plataforma = 0;
	ProgressBarX1.Minimum = 0;
	ProgressBarX1.Maximum = tbl_familia.Rows.Count;
	checked
	{
		int tbl_familia_Total = tbl_familia.Rows.Count - 1;
		int position = 0;
		while (true)
		{
			if (position > tbl_familia_Total)
			{
				break;
			}
			id_plataforma = Convert.ToInt32(tbl_familia.Rows[position]["id_plataforma"].ToString());
			if (id_plataforma == 0)
			{
				string nombre = tbl_familia.Rows[position]["nombre"].ToString().Trim();
				//string nombre_corregido = nombre;
				string nombre_corregido = slug_value(nombre);
				//int activo = 1;
				int activo = ((String.Compare(tbl_familia.Rows[position]["activo"].ToString(), "S",  false) == 0) ? 1 : 0);
				DataTable tbl_categories_id = await tb_Recordset_MySQL_Plataforma("SELECT id from categories where name = '" + nombre + "'", "categories");
				if (tbl_categories_id.Rows.Count <= 0)
				{
					if (await Insert_Plataforma("categories", "created_at = current_timestamp, updated_at = current_timestamp, image = 'no-photo.png',nivel = 0, parent_category = 0, name = '" + nombre + "', slug = '" + nombre_corregido + "', status = '" + Convert.ToString(activo) + "'"))
					{
						DataTable tbl_categories_lastId = tb_Recordset_MySQL_Plataforma("SELECT max(id) from categories", "categories");
						await Update_local("UPDATE rpa_t_familia SET sinc = 0, updated_at_plat = current_timestamp, id_plataforma = " + tbl_categories_lastId.Rows[0][0].ToString() + " WHERE id = " + tbl_familia.Rows[position]["id"].ToString(), "rpa_t_familia");
					}
					//goto IL_0361;
                    ProgressBarX1.Value += 1;
			        position++;
                    continue;
				}
				id_plataforma = Convert.ToInt32(tbl_categories_id.Rows[0][0].ToString());
				await Update_local("UPDATE rpa_t_familia SET updated_at_plat = current_timestamp, id_plataforma = " + Convert.ToString(id_plataforma) + " WHERE id = " + tbl_familia.Rows[position]["id"].ToString(), "rpa_t_familia");
			}
			string familia_nombre = tbl_familia.Rows[position]["nombre"].ToString().Trim();
			//string familia_nombre_Corregido = familia_nombre;
			string familia_nombre_Corregido = slug_value(familia_nombre);
			//int familia_Activo = 1;
			int familia_Activo = ((String.Compare(tbl_familia.Rows[position]["activo"].ToString(), "S",  false) == 0) ? 1 : 0);
			if (await Update_Plataforma("UPDATE categories SET updated_at = current_timestamp, name = '" + familia_nombre + "', status = '" + Convert.ToString(familia_Activo) + "' WHERE id = " + Convert.ToString(id_plataforma) + "", "categories"))
			{
				await Update_local("UPDATE rpa_t_familia SET updated_at_plat = current_timestamp, sinc = 0 WHERE id = " + tbl_familia.Rows[position]["id"].ToString(), "rpa_t_familia");
			}
			//goto IL_0361;
			//IL_0361:
			ProgressBarX1.Value += 1;
			position++;
		}
		ProgressBarX1.Value = 0;
	}
}
