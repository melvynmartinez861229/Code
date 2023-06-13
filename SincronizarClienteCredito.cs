// histoclin.frmMonitor
using System;
using System.Data;
using Microsoft.VisualBasic.CompilerServices;

private void SincronizarClienteCredito()
{
	DataTable tbl_cliente_credito = await tb_Recordset_MySQL_local("SELECT (Select    SUM(tc.debe)    FROM t_cliente_credito tc    WHERE    tc.id_cliente = rpa_t_cliente_credito.id_cliente AND    tc.credito_liquidado = 'N' AND    tc.cancelado = 'N') as cliente_credito_debe, (Select    SUM(CASE WHEN TIMESTAMPDIFF(DAY, DATE(NOW()), DATE(tc_nv.fecha_limite_de_pago)) > 7 THEN tc_nv.debe ELSE 0 END)    FROM t_cliente_credito tc_nv    WHERE    tc_nv.id_cliente = rpa_t_cliente_credito.id_cliente AND    tc_nv.credito_liquidado = 'N' AND    tc_nv.cancelado = 'N') as NO_VENCIDO, (Select    SUM(CASE WHEN TIMESTAMPDIFF(DAY, DATE(NOW()), DATE(tc_sd.fecha_limite_de_pago)) BETWEEN 1 AND 7 THEN tc_sd.debe ELSE 0 END)    FROM t_cliente_credito tc_sd    WHERE    tc_sd.id_cliente = rpa_t_cliente_credito.id_cliente AND    tc_sd.credito_liquidado = 'N' AND    tc_sd.cancelado = 'N') as VENCE_EN_7DIAS, (Select    SUM(CASE WHEN TIMESTAMPDIFF(DAY, DATE(NOW()), DATE(tc_tv.fecha_limite_de_pago)) < 0 THEN tc_tv.debe ELSE 0 END)    FROM t_cliente_credito tc_tv    WHERE    tc_tv.id_cliente = rpa_t_cliente_credito.id_cliente AND    tc_tv.credito_liquidado = 'N' AND    tc_tv.cancelado = 'N') as TOTAL_VENCIDO, rpa_t_cliente_credito.id, rpa_t_cliente_credito.id_cliente, rpa_t_cliente_credito.id_plataforma FROM rpa_t_cliente_credito WHERE rpa_t_cliente_credito.sinc = 1", "rpa_t_cliente_credito INNER t_cliente_credito");
	if (tbl_cliente_credito.Rows.Count == 0)
	{
		return;
	}
	int id_cliente = 0;
	int id_plataforma = 0;
	ProgressBarX1.Minimum = 0;
	ProgressBarX1.Maximum = tbl_cliente_credito.Rows.Count;
	checked
	{
		int cliente_credito_Total = tbl_cliente_credito.Rows.Count - 1;
		int position = 0;
		while (true)
		{
			if (position > cliente_credito_Total)
			{
				break;
			}
			id_cliente = Convert.ToInt32(tbl_cliente_credito.Rows[position]["id_cliente"].ToString());
			id_plataforma = Convert.ToInt32(tbl_cliente_credito.Rows[position]["id_plataforma"].ToString());

			double cliente_credito_debe = 0.0;
			try
			{
				cliente_credito_debe = Convert.ToDouble(tbl_cliente_credito.Rows[position]["cliente_credito_debe"].ToString());
			}
			catch (Exception ex)
			{
				cliente_credito_debe = 0.0;
			}

			double cliente_credito_NO_VENCIDO = 0.0;
			try
			{
				cliente_credito_NO_VENCIDO = Convert.ToDouble(tbl_cliente_credito.Rows[position]["NO_VENCIDO"].ToString());
			}
			catch (Exception ex3)
			{
				cliente_credito_NO_VENCIDO = 0.0;
			}

			double cliente_credito_VENCE_EN_7DIAS = 0.0;
			try
			{
				cliente_credito_VENCE_EN_7DIAS = Convert.ToDouble(tbl_cliente_credito.Rows[position]["VENCE_EN_7DIAS"].ToString());
			}
			catch (Exception ex5)
			{
				cliente_credito_VENCE_EN_7DIAS = 0.0;
			}

			double cliente_credito_TOTAL_VENCIDO = 0.0;
			try
			{
				cliente_credito_TOTAL_VENCIDO = Convert.ToDouble(tbl_cliente_credito.Rows[position]["TOTAL_VENCIDO"].ToString());
			}
			catch (Exception ex7)
			{
				cliente_credito_TOTAL_VENCIDO = 0.0;
			}

			if (id_plataforma == 0)
			{
				DataTable tbl_rpa_t_cliente_Id = await tb_Recordset_MySQL_local("SELECT id_plataforma from rpa_t_cliente WHERE id_cliente = " + Convert.ToString(id_cliente) + "", "rpa_t_cliente");
				if (tbl_rpa_t_cliente_Id.Rows.Count <= 0)
				{
                    ProgressBarX1.Value += 1;
			        position++;
                    continue;
				}
				id_plataforma = Convert.ToInt32(tbl_rpa_t_cliente_Id.Rows[0][0].ToString());

				if (id_plataforma <= 0)
				{
					ProgressBarX1.Value += 1;
			        position++;
                    continue;
				}
				await Update_local("UPDATE rpa_t_cliente_credito SET updated_at_plat = current_timestamp, id_plataforma = " + Convert.ToString(id_plataforma) + " WHERE id_cliente = " + Convert.ToString(id_cliente), "rpa_t_cliente_credito");
			}
			await Update_local("UPDATE rpa_t_cliente_credito SET updated_at_plat = current_timestamp WHERE id = " + tbl_cliente_credito.Rows[position]["id"].ToString(), "rpa_t_cliente_credito");
			if (await Update_Plataforma("UPDATE users SET updated_at = current_timestamp, cliente_credito_debe = '" + Convert.ToString(cliente_credito_debe) + "', no_vencido = '" + Convert.ToString(cliente_credito_NO_VENCIDO) + "', vence_siete_dias = '" + Convert.ToString(cliente_credito_VENCE_EN_7DIAS) + "', total_vencido = '" + Convert.ToString(cliente_credito_TOTAL_VENCIDO) + "'WHERE id = '" + Convert.ToString(id_plataforma) + "'", "users"))
			{
				await Update_local("UPDATE rpa_t_cliente_credito SET updated_at_plat = current_timestamp, sinc = 0 WHERE id = " + tbl_cliente_credito.Rows[position]["id"].ToString(), "rpa_t_cliente_credito");
			}
			
			ProgressBarX1.Value += 1;
			position++;
		}
		ProgressBarX1.Value = 0;
	}
}
