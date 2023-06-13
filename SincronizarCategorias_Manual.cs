// histoclin.frmMonitor
using System.Data;

private void SincronizarCategorias_Manual()
{
	DataTable tbl_rpa_t_familia_id = await tb_Recordset_MySQL_local("SELECT id from rpa_t_familia limit 1 ", "rpa_t_familia");
	if (tbl_rpa_t_familia_id.Rows.Count > 0)
	{
		return;
	}
	DataTable tbl_t_familia_id = await tb_Recordset_MySQL_local("SELECT t_familia.id_familia FROM t_familia WHERE t_familia.activo = 'S'", "t_familia");
	if (tbl_t_familia_id.Rows.Count == 0)
	{
		return;
	}
	//int num = 0;
	ProgressBarX1.Minimum = 0;
	ProgressBarX1.Maximum = tbl_t_familia_id.Rows.Count;
	checked
	{
		int tbl_t_familia_Total = tbl_t_familia_id.Rows.Count - 1;
		int position = 0;
		while (true)
		{
			//int num4 = position;
			//int num5 = tbl_t_familia_Total;
			if (position > tbl_t_familia_Total)
			{
				break;
			}
			await Insert_local("rpa_t_familia", "updated_at = current_timestamp, id_familia = '" + tbl_t_familia_id.Rows[position]["id_familia"].ToString() + "'");
			ProgressBarX1.Value += 1;
			position++;
		}
		ProgressBarX1.Value = 0;
	}
}
