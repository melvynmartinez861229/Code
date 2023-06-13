// histoclin.frmMonitor
using System.Data;

private void SincronizarProducto_Manual()
{
	DataTable tbl_rpa_t_producto_Id = await tb_Recordset_MySQL_local("SELECT id from rpa_t_producto limit 1 ", "rpa_t_producto");
	if (tbl_rpa_t_producto_Id.Rows.Count > 0)
	{
		return;
	}

	DataTable tbl_t_producto_id_product = await tb_Recordset_MySQL_local("SELECT t_producto.id_producto FROM t_producto WHERE t_producto.activo = 'S'", "t_producto");
	if (tbl_t_producto_id_product.Rows.Count == 0)
	{
		return;
	}
    
	ProgressBarX1.Minimum = 0;
	ProgressBarX1.Maximum = tbl_t_producto_id_product.Rows.Count;
	checked
	{
		int id_product_Total = tbl_t_producto_id_product.Rows.Count - 1;
		int position = 0;
		while (true)
		{
			int position = position;
			int id_product_Total = id_product_Total;
			if (position > id_product_Total)
			{
				break;
			}
			await Insert_local("rpa_t_producto", "updated_at = current_timestamp, id_producto = " + tbl_t_producto_id_product.Rows[position]["id_producto"].ToString() + "");
			await Insert_local("rpa_t_producto_foto", "updated_at = current_timestamp, id_producto = " + tbl_t_producto_id_product.Rows[position]["id_producto"].ToString() + "");
			ProgressBarX1.Value += 1;
			position++;
		}
		ProgressBarX1.Value = 0;
	}
}
