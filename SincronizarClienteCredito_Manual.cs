// histoclin.frmMonitor
using System.Data;

private void SincronizarClienteCredito_Manual()
{
	DataTable tbl_rpa_t_cliente = tb_Recordset_MySQL_local("SELECT rpa_t_cliente.id_cliente, rpa_t_cliente.id_plataforma FROM rpa_t_cliente", "rpa_t_cliente");
	if (tbl_rpa_t_cliente.Rows.Count == 0)
	{
		return;
	}

	ProgressBarX1.Minimum = 0;
	ProgressBarX1.Maximum = tbl_rpa_t_cliente.Rows.Count;
	checked
	{
		int cliente_Total = tbl_rpa_t_cliente.Rows.Count - 1;
		int position = 0;
		while (true)
		{
			if (position > cliente_Total)
			{
				break;
			}

			DataTable tbl_rpa_t_cliente_credito = await tb_Recordset_MySQL_local("SELECT id FROM rpa_t_cliente_credito where id_cliente = " + tbl_rpa_t_cliente.Rows[position]["id_cliente"].ToString() + "", "rpa_t_cliente_credito");
			if (tbl_rpa_t_cliente_credito.Rows.Count == 0)
			{
				await Insert_local("rpa_t_cliente_credito", "updated_at = current_timestamp, id_plataforma = " + tbl_rpa_t_cliente.Rows[position]["id_plataforma"].ToString() + ", id_cliente = " + tbl_rpa_t_cliente.Rows[position]["id_cliente"].ToString() + "");
			}
			else
			{
				await Update_local("UPDATE rpa_t_cliente_credito SET ", "updated_at = current_timestamp, id_plataforma = " + tbl_rpa_t_cliente.Rows[position]["id_plataforma"].ToString() + " WHERE id_cliente = " + tbl_rpa_t_cliente.Rows[position]["id_cliente"].ToString());
			}

			ProgressBarX1.Value += 1;
			position++;
		}
		ProgressBarX1.Value = 0;
	}
}
