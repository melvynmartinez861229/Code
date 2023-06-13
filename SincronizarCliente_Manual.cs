// histoclin.frmMonitor
using System.Data;

private void SincronizarCliente_Manual()
{
	DataTable tbl_rpa_t_cliente = await tb_Recordset_MySQL_local("SELECT id from rpa_t_cliente limit 1 ", "rpa_t_cliente");
	if (tbl_rpa_t_cliente.Rows.Count > 0)
	{
		return;
	}
	DataTable tbl_t_cliente_id = await tb_Recordset_MySQL_local("SELECT t_cliente.id_cliente FROM t_cliente WHERE t_cliente.activo = 'S'", "t_cliente");
	if (tbl_t_cliente_id.Rows.Count == 0)
	{
		return;
	}
	//int num = 0;
	ProgressBarX1.Minimum = 0;
	ProgressBarX1.Maximum = tbl_t_cliente_id.Rows.Count;
	checked
	{
		int cliente_id_Total = tbl_t_cliente_id.Rows.Count - 1;
		int position = 0;
		while (true)
		{
			if (position > cliente_id_Total)
			{
				break;
			}
			await Insert_local("rpa_t_cliente", "updated_at = current_timestamp, id_cliente = " + tbl_t_cliente_id.Rows[position]["id_cliente"].ToString() + "");
			ProgressBarX1.Value += 1;
			position++;
		}
		ProgressBarX1.Value = 0;
	}
}
