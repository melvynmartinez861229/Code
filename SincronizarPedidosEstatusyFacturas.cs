// histoclin.frmMonitor
using System.Data;
using Microsoft.VisualBasic.CompilerServices;

private void SincronizarPedidosEstatusyFacturas()
{
	//Discarded unreachable code: IL_0168, IL_018c
	DataTable tbl_rpa_t_pedido = tb_Recordset_MySQL_local("SELECT t_pedido.estatus_web, rpa_t_pedido.id, rpa_t_pedido.id_pedido, rpa_t_pedido.id_plataforma FROM rpa_t_pedido INNER JOIN t_pedido ON (t_pedido.id_pedido = rpa_t_pedido.id_pedido) WHERE rpa_t_pedido.sinc = 1", "rpa_t_pedido INNER t_pedido");
	if (tbl_rpa_t_pedido.Rows.Count == 0)
	{
		return;
	}
	int id_plataforma = 0;
	ProgressBarX1.Minimum = 0;
	ProgressBarX1.Maximum = tbl_rpa_t_pedido.Rows.Count;
	checked
	{
		int tbl_rpa_t_pedido_Total = tbl_rpa_t_pedido.Rows.Count - 1;
		int position = 0;

		string estatus_web = "";
		string id_pedido = "";

		while (true)
		{
			if (position > tbl_rpa_t_pedido_Total)
			{
				break;
			}
			id_plataforma = Conversions.ToInteger(tbl_rpa_t_pedido.Rows[position]["id_plataforma"].ToString());
			if (id_plataforma == 0)
			{
				estatus_web = tbl_rpa_t_pedido.Rows[position]["estatus_web"].ToString().Trim();
				id_pedido = tbl_rpa_t_pedido.Rows[position]["id_pedido"].ToString().Trim();
				DataTable tbl_shopping = tb_Recordset_MySQL_Plataforma("SELECT id from shopping where folio_local = '" + id_pedido + "'", "shopping");
				if (tbl_shopping.Rows.Count <= 0)
				{
					Log_DB("No se encontró folio local: " + id_pedido + " asociado a un pedido en la plataforma", "shopping");
					ProgressBarX1.Value += 1;
					position++;
					continue;
				}
				id_plataforma = Conversions.ToInteger(tbl_shopping.Rows[0][0].ToString());
				Update_local("UPDATE rpa_t_pedido SET updated_at_plat = current_timestamp, id_plataforma = " + Conversions.ToString(id_plataforma) + " WHERE id = " + tbl_rpa_t_pedido.Rows[position]["id"].ToString(), "rpa_t_pedido");
			}

			estatus_web = tbl_rpa_t_pedido.Rows[position]["estatus_web"].ToString().Trim();
			id_pedido = tbl_rpa_t_pedido.Rows[position]["id_pedido"].ToString().Trim();

			if (Update_Plataforma("UPDATE shopping SET updated_at = current_timestamp, shopping_status = '" + estatus_web + "' WHERE id = '" + Conversions.ToString(id_plataforma) + "'", "shopping"))
			{
				Update_local("UPDATE rpa_t_pedido SET updated_at_plat = current_timestamp, sinc = 0 WHERE id = " + tbl_rpa_t_pedido.Rows[position]["id"].ToString().Trim(), "rpa_t_pedido");
			}
			if (!InsertarFactura(Conversions.ToInteger(id_pedido), id_plataforma, Conversions.ToInteger(tbl_rpa_t_pedido.Rows[position]["id"].ToString().Trim())))
			{
			}
			ProgressBarX1.Value += 1;
			position++;
		}
		ProgressBarX1.Value = 0;
	}
}
