// histoclin.frmMonitor
using System.Data;

private void SincronizarLaboratorios_Manual()
{
	DataTable tbl_rpa_t_laboratorio = tb_Recordset_MySQL_local("SELECT id from rpa_t_laboratorio limit 1 ", "rpa_t_laboratorio");
	if (tbl_rpa_t_laboratorio.Rows.Count > 0)
	{
		return;
	}
	DataTable tbl_t_laboratorio_Id = tb_Recordset_MySQL_local("SELECT t_laboratorio.id_laboratorio FROM t_laboratorio WHERE t_laboratorio.activo = 'S'", "t_laboratorio");
	if (tbl_t_laboratorio_Id.Rows.Count == 0)
	{
		return;
	}

	ProgressBarX1.Minimum = 0;
	ProgressBarX1.Maximum = tbl_t_laboratorio_Id.Rows.Count;
	checked
	{
		int laboratorio_Total = tbl_t_laboratorio_Id.Rows.Count - 1;
		int position = 0;
		while (true)
		{
			if (position > laboratorio_Total)
			{
				break;
			}
			await Insert_local("rpa_t_laboratorio", "updated_at = current_timestamp, id_laboratorio = " + tbl_t_laboratorio_Id.Rows[position]["id_laboratorio"].ToString() + "");
			ProgressBarX1.Value += 1;
			position++;
		}
		ProgressBarX1.Value = 0;
	}
}
