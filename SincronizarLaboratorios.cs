// histoclin.frmMonitor
using System.Data;
using Microsoft.VisualBasic.CompilerServices;

private void SincronizarLaboratorios()
{
	DataTable tbl_t_laboratorio = tb_Recordset_MySQL_local("SELECT t_laboratorio.clave, t_laboratorio.nombre, t_laboratorio.activo, rpa_t_laboratorio.id, rpa_t_laboratorio.id_laboratorio, rpa_t_laboratorio.id_plataforma FROM rpa_t_laboratorio INNER JOIN t_laboratorio ON (t_laboratorio.id_laboratorio = rpa_t_laboratorio.id_laboratorio) WHERE rpa_t_laboratorio.sinc = 1", "rpa_t_laboratorio INNER t_laboratorio");
	if (tbl_t_laboratorio.Rows.Count == 0)
	{
		return;
	}
	int id_plataforma = 0;
	ProgressBarX1.Minimum = 0;
	ProgressBarX1.Maximum = tbl_t_laboratorio.Rows.Count;
	checked
	{
		int t_laboratorio_Total = tbl_t_laboratorio.Rows.Count - 1;
		int position = 0;
		while (true)
		{
			if (position > t_laboratorio_Total)
			{
				break;
			}

            string nombre = tbl_t_laboratorio.Rows[position]["nombre"].ToString().Trim();
            string nombreCorregido = slug_value(nombre);
            
			id_plataforma = Convert.ToInt32(tbl_t_laboratorio.Rows[position]["id_plataforma"].ToString());
			if (id_plataforma == 0)
			{
				
				DataTable tbl_brands = await tb_Recordset_MySQL_Plataforma("SELECT id from brands where name = '" + nombre + "'", "brands");
				if (tbl_brands.Rows.Count <= 0)
				{
					if (await Insert_Plataforma("brands", "created_at = current_timestamp, updated_at = current_timestamp, name = '" + nombre + "', slug = '" + nombreCorregido + "'"))
					{
						DataTable tbl_brands_LastId = await tb_Recordset_MySQL_Plataforma("SELECT max(id) from brands", "brands");
						await Update_local("UPDATE rpa_t_laboratorio SET sinc = 0, updated_at_plat = current_timestamp, id_plataforma = " + tbl_brands_LastId.Rows[0][0].ToString() + " WHERE id = " + tbl_t_laboratorio.Rows[position]["id"].ToString(), "rpa_t_laboratorio");
					}
					ProgressBarX1.Value += 1;
			        position++;
                    continue;
				}
				id_plataforma = Convert.ToInt32(tbl_brands.Rows[0][0].ToString());
				await Update_local("UPDATE rpa_t_laboratorio SET updated_at_plat = current_timestamp, id_plataforma = " + Convert.ToString(id_plataforma) + " WHERE id = " + tbl_t_laboratorio.Rows[position]["id"].ToString(), "rpa_t_laboratorio");
			}
			
			if (await Update_Plataforma("UPDATE brands SET updated_at = current_timestamp, name = '" + nombre + "' WHERE id = '" + Convert.ToString(id_plataforma) + "'", "brands"))
			{
				await Update_local("UPDATE rpa_t_laboratorio SET updated_at_plat = current_timestamp, sinc = 0 WHERE id = " + tbl_t_laboratorio.Rows[position]["id"].ToString(), "rpa_t_laboratorio");
			}
			
			ProgressBarX1.Value += 1;
			position++;
		}
		ProgressBarX1.Value = 0;
	}
}
