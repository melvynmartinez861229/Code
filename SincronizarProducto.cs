// histoclin.frmMonitor
using System;
using System.Data;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;

private void SincronizarProducto()
{
	string clave = "";
	string clave_2 = "";
	string nombre = "";
	string nombreCorregido = "";
	int activo = 1;
	string iva = "";
	string sustancia_activa = "";
	string id_laboratorio = "176";
	int cantidad_bulto_original = 0;
	string temporada_oferta_1 = "";
	string current_timestamp = "";
	string descuento_oferta_1 = "";
	string concentracion = "";
	string presentacion = "";
	string contenido = "";
	string sustancia_activa_1 = "";
	string sustancia_activa_2 = "";
	string sustancia_activa_3 = "";
	string sustancia_activa_4 = "";
	string sustancia_activa_5 = "";
	string sustancia_activa_6 = "";
	string sustancia_activa_7 = "";
	
    DataTable tbl_rpa_t_producto = await tb_Recordset_MySQL_local("SELECT t_producto.clave, t_producto.clave_2, t_producto.nombre, t_producto.activo, t_producto.iva, t_producto.fecha_baja, t_producto.id_familia, t_producto.precio_venta_publico, t_producto.existencia, t_producto.descuento_1, t_producto.descuento_2, t_producto.descuento_3, t_producto.descuento_4, t_producto.descuento_5, t_producto.descuento_6, t_producto.descuento_7, t_producto.descuento_8, t_producto.descuento_9, t_producto.descuento_10, t_producto.descuento_11, t_producto.descuento_12, t_producto.descuento_13, t_producto.descuento_14, t_producto.sustancia_activa, t_producto.id_laboratorio, t_producto.cantidad_bulto_original, t_producto.temporada_oferta_1, t_producto.descuento_oferta_1, t_producto.id_producto AS id_producto_local, t_producto.concentracion, t_producto.presentacion, t_producto.contenido, t_producto.sustancia_activa_1, t_producto.sustancia_activa_2, t_producto.sustancia_activa_3, t_producto.sustancia_activa_4, t_producto.sustancia_activa_5, t_producto.sustancia_activa_6, t_producto.sustancia_activa_7, rpa_t_producto.id, rpa_t_producto.id_producto, rpa_t_producto.id_plataforma FROM rpa_t_producto INNER JOIN t_producto ON (t_producto.id_producto = rpa_t_producto.id_producto) WHERE rpa_t_producto.sinc = 1", "rpa_t_producto INNER t_producto");
	if (tbl_rpa_t_producto.Rows.Count == 0)
	{
		return;
	}

	int id_plataforma = 0;
	ProgressBarX1.Minimum = 0;
	ProgressBarX1.Maximum = tbl_rpa_t_producto.Rows.Count;
	checked
	{
		int tbl_rpa_t_producto_Total = tbl_rpa_t_producto.Rows.Count - 1;
		int position = 0;
		while (true)
		{
			
			if (position > tbl_rpa_t_producto_Total)
			{
				break;
			}

			DataTable tbl_rpa_t_producto_id = await tb_Recordset_MySQL_local("SELECT id_producto from rpa_t_producto_foto where id_producto = " + tbl_rpa_t_producto.Rows[position]["id_producto_local"].ToString().Trim() + "", "rpa_t_producto_foto de t_producto");
			if (tbl_rpa_t_producto_id.Rows.Count == 0)
			{
				 await Insert_local("rpa_t_producto_foto", "updated_at = current_timestamp, id_producto = " + tbl_rpa_t_producto.Rows[position]["id_producto_local"].ToString() + "");
			}

			id_plataforma = Convert.ToInt32(tbl_rpa_t_producto.Rows[position]["id_plataforma"].ToString());
			if (id_plataforma == 0)
			{
				id_laboratorio = tbl_rpa_t_producto.Rows[position]["id_laboratorio"].ToString().Trim();
				sustancia_activa = tbl_rpa_t_producto.Rows[position]["sustancia_activa"].ToString().Trim();
				clave = tbl_rpa_t_producto.Rows[position]["clave"].ToString().Trim();
				clave_2 = tbl_rpa_t_producto.Rows[position]["clave_2"].ToString().Trim();
				nombre = tbl_rpa_t_producto.Rows[position]["nombre"].ToString().Trim();
				concentracion = tbl_rpa_t_producto.Rows[position]["concentracion"].ToString().Trim();
				presentacion = tbl_rpa_t_producto.Rows[position]["presentacion"].ToString().Trim();
				contenido = tbl_rpa_t_producto.Rows[position]["contenido"].ToString().Trim();
				sustancia_activa_1 = tbl_rpa_t_producto.Rows[position]["sustancia_activa_1"].ToString().Trim();
				sustancia_activa_2 = tbl_rpa_t_producto.Rows[position]["sustancia_activa_2"].ToString().Trim();
				sustancia_activa_3 = tbl_rpa_t_producto.Rows[position]["sustancia_activa_3"].ToString().Trim();
				sustancia_activa_4 = tbl_rpa_t_producto.Rows[position]["sustancia_activa_4"].ToString().Trim();
				sustancia_activa_5 = tbl_rpa_t_producto.Rows[position]["sustancia_activa_5"].ToString().Trim();
				sustancia_activa_6 = tbl_rpa_t_producto.Rows[position]["sustancia_activa_6"].ToString().Trim();
				sustancia_activa_7 = tbl_rpa_t_producto.Rows[position]["sustancia_activa_7"].ToString().Trim();
				nombreCorregido = nombre;
				nombreCorregido = slug_value(nombreCorregido);
				activo = ((Operators.Compare(tbl_rpa_t_producto.Rows[position]["activo"].ToString(), "S",  false) == 0) ? 1 : 0);
				iva = tbl_rpa_t_producto.Rows[position]["iva"].ToString();
				DataTable dataTable3 =  await tb_Recordset_MySQL_Plataforma("SELECT id from products where sku = '" + clave + "'", "products");
				if (dataTable3.Rows.Count <= 0)
				{
					try
					{
						if (Convert.ToDouble(id_laboratorio) == 0.0)
						{
							id_laboratorio = Convert.ToString(176);
						}
					}
					catch (Exception ex)
					{
						id_laboratorio = Convert.ToString(176);						
					}


					cantidad_bulto_original = Convert.ToInt32(tbl_rpa_t_producto.Rows[position]["cantidad_bulto_original"].ToString().Trim());
					temporada_oferta_1 = tbl_rpa_t_producto.Rows[position]["temporada_oferta_1"].ToString().Trim();
					try
					{
						 await Update_local("UPDATE t_producto SET fecha_inicio_oferta_1 = NULL  WHERE fecha_inicio_oferta_1 = '0000-00-00' AND id_producto = " + tbl_rpa_t_producto.Rows[position]["id_producto_local"].ToString() + "", "t_producto");
						DataTable tbl_t_producto_id_local =  await tb_Recordset_MySQL_local("SELECT fecha_inicio_oferta_1 from t_producto WHERE id_producto = " + tbl_rpa_t_producto.Rows[position]["id_producto_local"].ToString() + "", "t_producto fecha_inicio_oferta_1 id_producto " + tbl_rpa_t_producto.Rows[position]["id_producto_local"].ToString());
						if ((Operators.Compare(tbl_t_producto_id_local.Rows[0]["fecha_inicio_oferta_1"].ToString(), "0000-00-00",  false) == 0) | (Operators.Compare(tbl_t_producto_id_local.Rows[0]["fecha_inicio_oferta_1"].ToString(), "",  false) == 0))
						{
							current_timestamp = "current_timestamp";
						}
						else
						{
							DateTime dateTime = Convert.ToDateTime(tbl_t_producto_id_local.Rows[0]["fecha_inicio_oferta_1"].ToString());
							current_timestamp = "'" + Strings.Format(dateTime, "yyyy-MM-dd") + "'";
						}
					}
					catch (Exception ex3)
					{
						current_timestamp = "current_timestamp";
					}
					try
					{
						 await Update_local("UPDATE t_producto SET fecha_termino_oferta_1 = NULL  WHERE fecha_termino_oferta_1 = '0000-00-00' AND id_producto = " + tbl_rpa_t_producto.Rows[position]["id_producto_local"].ToString() + "", "t_producto");
						DataTable tbl_t_producto_Id_Local =  await tb_Recordset_MySQL_local("SELECT fecha_termino_oferta_1 from t_producto WHERE id_producto = " + tbl_rpa_t_producto.Rows[position]["id_producto_local"].ToString() + "", "t_producto fecha_termino_oferta_1 id_producto " + tbl_rpa_t_producto.Rows[position]["id_producto_local"].ToString());
						if ((Operators.Compare(tbl_t_producto_Id_Local.Rows[0]["fecha_termino_oferta_1"].ToString(), "0000-00-00",  false) == 0) | (Operators.Compare(tbl_t_producto_Id_Local.Rows[0]["fecha_termino_oferta_1"].ToString(), "",  false) == 0))
						{
							current_timestamp = "current_timestamp";
						}
						else
						{
							DateTime tbl_fecha_termino_oferta_1 = Convert.ToDateTime(tbl_t_producto_Id_Local.Rows[0]["fecha_termino_oferta_1"].ToString());
							current_timestamp = "'" + Strings.Format(tbl_fecha_termino_oferta_1, "yyyy-MM-dd") + "'";
						}
					}
					catch (Exception ex5)
					{						
						current_timestamp = "current_timestamp";
					}


					descuento_oferta_1 = tbl_rpa_t_producto.Rows[position]["descuento_oferta_1"].ToString().Trim();
					if ( await Insert_Plataforma("products", "created_at = current_timestamp, updated_at = current_timestamp, images = 'no-photo.png',cover_image = 'no-photo.png',department = '" + id_laboratorio + "', piezas_bulto_original = '" + Convert.ToString(cantidad_bulto_original) + "', temporada_oferta_1 = '" + temporada_oferta_1 + "', fecha_inicio_oferta_1 = " + current_timestamp + ", fecha_termino_oferta_1 = " + current_timestamp + ", descuento_oferta_1 = '" + descuento_oferta_1 + "', description = '" + sustancia_activa + "', sku = '" + clave + "', alterna = '" + clave_2 + "', name = '" + nombre + "', slug = '" + nombreCorregido + "', status = '" + Convert.ToString(activo) + "', concentracion = '" + concentracion + "', presentacion = '" + presentacion + "', contenido = '" + contenido + "', sustancia_activa_1 = '" + sustancia_activa_1 + "', sustancia_activa_2 = '" + sustancia_activa_2 + "', sustancia_activa_3 = '" + sustancia_activa_3 + "', sustancia_activa_4 = '" + sustancia_activa_4 + "', sustancia_activa_5 = '" + sustancia_activa_5 + "', sustancia_activa_6 = '" + sustancia_activa_6 + "', sustancia_activa_7 = '" + sustancia_activa_7 + "', iva = '" + iva + "'"))
					{
						DataTable products_Last_Id =  await tb_Recordset_MySQL_Plataforma("SELECT max(id) from products", "products");
						  await Update_local("UPDATE rpa_t_producto SET sinc = 0, updated_at_plat = current_timestamp, id_plataforma = " + products_Last_Id.Rows[0][0].ToString() + " WHERE id = " + tbl_rpa_t_producto.Rows[position]["id"].ToString(), "rpa_t_producto");
						int products_Last_Id_Int = Convert.ToInt32(products_Last_Id.Rows[0][0].ToString());
						int category_id = 0;
						
                        DataTable tbl_rpa_t_familia_ID = await tb_Recordset_MySQL_local("SELECT id_plataforma from rpa_t_familia where id_familia = " + tbl_rpa_t_producto.Rows[position]["id_familia"].ToString() + "", "rpa_t_familia");
						if (tbl_rpa_t_familia_ID.Rows.Count == 0)
						{
							category_id = 40;
						}
						else
						{
							category_id = Convert.ToInt32(tbl_rpa_t_familia_ID.Rows[0][0].ToString());
							if (category_id == 0)
							{
								category_id = 40;
							}
							 await Insert_Plataforma("categories_products", "product_id = '" + Convert.ToString(products_Last_Id_Int) + "', category_id = '" + Convert.ToString(category_id) + "'");
						}

						double existencia = Convert.ToDouble(tbl_rpa_t_producto.Rows[position]["existencia"].ToString());
						double precio_venta_publico = Convert.ToDouble(tbl_rpa_t_producto.Rows[position]["precio_venta_publico"].ToString());
						if (Convert.ToDouble(iva) != 0.0)
						{
							precio_venta_publico /= 1.0 + Convert.ToDouble(iva);
							precio_venta_publico = Math.Round(precio_venta_publico, 6);
						}
						double num11 = 0.0;
						num11 = precio_venta_publico;
						 await Insert_Plataforma("products_sucursals", "sucursal_id = '" + Convert.ToString(SUCURSAL_ID) + "', product_id = '" + Convert.ToString(products_Last_Id_Int) + "', quantity = '" + Convert.ToString(existencia) + "', price = '" + Convert.ToString(num11) + "'");
						
                        double descuento_1 = Convert.ToDouble(tbl_rpa_t_producto.Rows[position]["descuento_1"].ToString());
						double descuento_2 = Convert.ToDouble(tbl_rpa_t_producto.Rows[position]["descuento_2"].ToString());
						double descuento_3 = Convert.ToDouble(tbl_rpa_t_producto.Rows[position]["descuento_3"].ToString());
						double descuento_4 = Convert.ToDouble(tbl_rpa_t_producto.Rows[position]["descuento_4"].ToString());
						double descuento_5 = Convert.ToDouble(tbl_rpa_t_producto.Rows[position]["descuento_5"].ToString());
						double descuento_6 = Convert.ToDouble(tbl_rpa_t_producto.Rows[position]["descuento_6"].ToString());
						double descuento_7 = Convert.ToDouble(tbl_rpa_t_producto.Rows[position]["descuento_7"].ToString());
						double descuento_8 = Convert.ToDouble(tbl_rpa_t_producto.Rows[position]["descuento_8"].ToString());
						double descuento_9 = Convert.ToDouble(tbl_rpa_t_producto.Rows[position]["descuento_9"].ToString());
						double descuento_10 = Convert.ToDouble(tbl_rpa_t_producto.Rows[position]["descuento_10"].ToString());
						double descuento_11 = Convert.ToDouble(tbl_rpa_t_producto.Rows[position]["descuento_11"].ToString());
						double descuento_12 = Convert.ToDouble(tbl_rpa_t_producto.Rows[position]["descuento_12"].ToString());
						double descuento_13 = Convert.ToDouble(tbl_rpa_t_producto.Rows[position]["descuento_13"].ToString());
						double descuento_14 = Convert.ToDouble(tbl_rpa_t_producto.Rows[position]["descuento_14"].ToString());
						 await Insert_Plataforma("discount_product", "created_at = current_timestamp, updated_at = current_timestamp, product_id = '" + Convert.ToString(products_Last_Id_Int) + "', level_1 = '" + Convert.ToString(descuento_1) + "', level_2 = '" + Convert.ToString(descuento_2) + "', level_3 = '" + Convert.ToString(descuento_3) + "', level_4 = '" + Convert.ToString(descuento_4) + "', level_5 = '" + Convert.ToString(descuento_5) + "', level_6 = '" + Convert.ToString(descuento_6) + "', level_7 = '" + Convert.ToString(descuento_7) + "', level_8 = '" + Convert.ToString(descuento_8) + "', level_9 = '" + Convert.ToString(descuento_9) + "', level_10 = '" + Convert.ToString(descuento_10) + "', level_11 = '" + Convert.ToString(descuento_11) + "', level_12 = '" + Convert.ToString(descuento_12) + "', level_13 = '" + Convert.ToString(descuento_13) + "', level_14 = '" + Convert.ToString(descuento_14) + "'");
					}
					ProgressBarX1.Value += 1;
			        position++;
                    continue;
				}
				id_plataforma = Convert.ToInt32(dataTable3.Rows[0][0].ToString());
				 await Update_local("UPDATE rpa_t_producto SET updated_at_plat = current_timestamp, id_plataforma = " + Convert.ToString(id_plataforma) + " WHERE id = " + tbl_rpa_t_producto.Rows[position]["id"].ToString(), "rpa_t_producto");
			}
			id_laboratorio = tbl_rpa_t_producto.Rows[position]["id_laboratorio"].ToString().Trim();
			sustancia_activa = tbl_rpa_t_producto.Rows[position]["sustancia_activa"].ToString().Trim();
			clave = tbl_rpa_t_producto.Rows[position]["clave"].ToString().Trim();
			clave_2 = tbl_rpa_t_producto.Rows[position]["clave_2"].ToString().Trim();
			nombre = tbl_rpa_t_producto.Rows[position]["nombre"].ToString().Trim();
			concentracion = tbl_rpa_t_producto.Rows[position]["concentracion"].ToString().Trim();
			presentacion = tbl_rpa_t_producto.Rows[position]["presentacion"].ToString().Trim();
			contenido = tbl_rpa_t_producto.Rows[position]["contenido"].ToString().Trim();
			sustancia_activa_1 = tbl_rpa_t_producto.Rows[position]["sustancia_activa_1"].ToString().Trim();
			sustancia_activa_2 = tbl_rpa_t_producto.Rows[position]["sustancia_activa_2"].ToString().Trim();
			sustancia_activa_3 = tbl_rpa_t_producto.Rows[position]["sustancia_activa_3"].ToString().Trim();
			sustancia_activa_4 = tbl_rpa_t_producto.Rows[position]["sustancia_activa_4"].ToString().Trim();
			sustancia_activa_5 = tbl_rpa_t_producto.Rows[position]["sustancia_activa_5"].ToString().Trim();
			sustancia_activa_6 = tbl_rpa_t_producto.Rows[position]["sustancia_activa_6"].ToString().Trim();
			sustancia_activa_7 = tbl_rpa_t_producto.Rows[position]["sustancia_activa_7"].ToString().Trim();
			nombreCorregido = nombre;
			nombreCorregido = slug_value(nombreCorregido);
			activo = ((Operators.Compare(tbl_rpa_t_producto.Rows[position]["activo"].ToString(), "S",  false) == 0) ? 1 : 0);
			iva = tbl_rpa_t_producto.Rows[position]["iva"].ToString();

			try
			{
				if (Convert.ToDouble(id_laboratorio) == 0.0)
				{
					id_laboratorio = Convert.ToString(176);
				}
			}
			catch (Exception ex7)
			{
				id_laboratorio = Convert.ToString(176);
			}

			cantidad_bulto_original = await Convert.ToInt32(tbl_rpa_t_producto.Rows[position]["cantidad_bulto_original"].ToString().Trim());
			temporada_oferta_1 = tbl_rpa_t_producto.Rows[position]["temporada_oferta_1"].ToString().Trim();
			try
			{
				 await Update_local("UPDATE t_producto SET fecha_inicio_oferta_1 = NULL  WHERE fecha_inicio_oferta_1 = '0000-00-00' AND id_producto = " + tbl_rpa_t_producto.Rows[position]["id_producto_local"].ToString() + "", "t_producto");
				DataTable tbl_t_producto_ID = await tb_Recordset_MySQL_local("SELECT fecha_inicio_oferta_1 from t_producto where id_producto = " + tbl_rpa_t_producto.Rows[position]["id_producto_local"].ToString() + "", "t_producto fecha_inicio_oferta_1 id_producto " + tbl_rpa_t_producto.Rows[position]["id_producto_local"].ToString());
				if ((Operators.Compare(tbl_t_producto_ID.Rows[0]["fecha_inicio_oferta_1"].ToString(), "0000-00-00",  false) == 0) | (Operators.Compare(tbl_t_producto_ID.Rows[0]["fecha_inicio_oferta_1"].ToString(), "",  false) == 0))
				{
					current_timestamp = "current_timestamp";
				}
				else
				{
					DateTime fecha_inicio_oferta_1 = Convert.ToDateTime(tbl_t_producto_ID.Rows[0]["fecha_inicio_oferta_1"].ToString());
					current_timestamp = "'" + Strings.Format(fecha_inicio_oferta_1, "yyyy-MM-dd") + "'";
				}
			}
			catch (Exception ex9)
			{
				current_timestamp = "current_timestamp";
			}

			try
			{
				 await Update_local("UPDATE t_producto SET fecha_termino_oferta_1 = NULL  WHERE fecha_termino_oferta_1 = '0000-00-00' AND id_producto = " + tbl_rpa_t_producto.Rows[position]["id_producto_local"].ToString() + "", "t_producto");
				DataTable tbl_t_producto_Id = await tb_Recordset_MySQL_local("SELECT fecha_termino_oferta_1 from t_producto where id_producto = " + tbl_rpa_t_producto.Rows[position]["id_producto_local"].ToString() + "", "t_producto fecha_termino_oferta_1 id_producto " + tbl_rpa_t_producto.Rows[position]["id_producto_local"].ToString());
				if ((Operators.Compare(tbl_t_producto_Id.Rows[0]["fecha_termino_oferta_1"].ToString(), "0000-00-00",  false) == 0) | (Operators.Compare(tbl_t_producto_Id.Rows[0]["fecha_termino_oferta_1"].ToString(), "",  false) == 0))
				{
					current_timestamp = "current_timestamp";
				}
				else
				{
					DateTime fecha_termino_oferta_1 = Convert.ToDateTime(tbl_t_producto_Id.Rows[0]["fecha_termino_oferta_1"].ToString());
					current_timestamp = "'" + Strings.Format(fecha_termino_oferta_1, "yyyy-MM-dd") + "'";
				}
			}

			catch (Exception ex11)
			{
				current_timestamp = "current_timestamp";
			}

			descuento_oferta_1 = tbl_rpa_t_producto.Rows[position]["descuento_oferta_1"].ToString().Trim();
			if ( await Update_Plataforma("UPDATE products SET updated_at = current_timestamp, department = '" + id_laboratorio + "', piezas_bulto_original = '" + Convert.ToString(cantidad_bulto_original) + "', temporada_oferta_1 = '" + temporada_oferta_1 + "', fecha_inicio_oferta_1 = " + current_timestamp + ", fecha_termino_oferta_1 = " + current_timestamp + ", descuento_oferta_1 = '" + descuento_oferta_1 + "', description = '" + sustancia_activa + "', alterna = '" + clave_2 + "', name = '" + nombre + "', status = '" + Convert.ToString(activo) + "', concentracion = '" + concentracion + "', presentacion = '" + presentacion + "', contenido = '" + contenido + "', sustancia_activa_1 = '" + sustancia_activa_1 + "', sustancia_activa_2 = '" + sustancia_activa_2 + "', sustancia_activa_3 = '" + sustancia_activa_3 + "', sustancia_activa_4 = '" + sustancia_activa_4 + "', sustancia_activa_5 = '" + sustancia_activa_5 + "', sustancia_activa_6 = '" + sustancia_activa_6 + "', sustancia_activa_7 = '" + sustancia_activa_7 + "', iva = '" + iva + "' WHERE id = '" + Convert.ToString(id_plataforma) + "'", "products"))
			{
				 await Update_local("UPDATE rpa_t_producto SET updated_at_plat = current_timestamp, sinc = 0 WHERE id = " + tbl_rpa_t_producto.Rows[position]["id"].ToString(), "rpa_t_producto");
				int category_id = 0;
				DataTable dataTable10 = await tb_Recordset_MySQL_local("SELECT id_plataforma from rpa_t_familia where id_familia = " + tbl_rpa_t_producto.Rows[position]["id_familia"].ToString() + "", "rpa_t_familia");
				if (dataTable10.Rows.Count == 0)
				{
					category_id = 40;
				}
				else
				{
					category_id = Convert.ToInt32(dataTable10.Rows[0][0].ToString());
					if (category_id == 0)
					{
						category_id = 40;
					}
					DataTable tbl_categories_products_Id = await tb_Recordset_MySQL_Plataforma("SELECT * from categories_products where product_id = '" + Convert.ToString(id_plataforma) + "'", "categories_products");
					if (tbl_categories_products_Id.Rows.Count == 0)
					{
						 await Insert_Plataforma("categories_products", "product_id = '" + Convert.ToString(id_plataforma) + "', category_id = '" + Convert.ToString(category_id) + "'");
					}
					else
					{
						 await Update_Plataforma("UPDATE categories_products SET category_id = '" + Convert.ToString(category_id) + "' WHERE product_id = '" + Convert.ToString(id_plataforma) + "'", "categories_products");
					}
				}

				double existencia = Convert.ToDouble(tbl_rpa_t_producto.Rows[position]["existencia"].ToString());
				double precio_venta_publico = Convert.ToDouble(tbl_rpa_t_producto.Rows[position]["precio_venta_publico"].ToString());
				if (Convert.ToDouble(iva) != 0.0)
				{
					precio_venta_publico /= 1.0 + Convert.ToDouble(iva);
					precio_venta_publico = Math.Round(precio_venta_publico, 6);
				}

				double price = 0.0;
				price = precio_venta_publico;
				DataTable tbl_products_sucursals_Id =  await tb_Recordset_MySQL_Plataforma("SELECT product_id from products_sucursals where product_id = '" + Convert.ToString(id_plataforma) + "' and sucursal_id = '" + Convert.ToString(SUCURSAL_ID) + "'", "products_sucursals");
				if (tbl_products_sucursals_Id.Rows.Count == 0)
				{
					 await Insert_Plataforma("products_sucursals", "sucursal_id = '" + Convert.ToString(SUCURSAL_ID) + "', product_id = '" + Convert.ToString(id_plataforma) + "', quantity = '" + Convert.ToString(existencia) + "', price = '" + Convert.ToString(price) + "'");
				}
				else
				{
					 await Update_Plataforma("UPDATE products_sucursals SET quantity = '" + Convert.ToString(existencia) + "', price = '" + Convert.ToString(price) + "' WHERE product_id = '" + Convert.ToString(id_plataforma) + "' and sucursal_id = '" + Convert.ToString(SUCURSAL_ID) + "'", "categories_products");
				}
				double descuento_1 = Convert.ToDouble(tbl_rpa_t_producto.Rows[position]["descuento_1"].ToString());
				double descuento_2 = Convert.ToDouble(tbl_rpa_t_producto.Rows[position]["descuento_2"].ToString());
				double descuento_3 = Convert.ToDouble(tbl_rpa_t_producto.Rows[position]["descuento_3"].ToString());
				double descuento_4 = Convert.ToDouble(tbl_rpa_t_producto.Rows[position]["descuento_4"].ToString());
				double descuento_5 = Convert.ToDouble(tbl_rpa_t_producto.Rows[position]["descuento_5"].ToString());
				double descuento_6 = Convert.ToDouble(tbl_rpa_t_producto.Rows[position]["descuento_6"].ToString());
				double descuento_7 = Convert.ToDouble(tbl_rpa_t_producto.Rows[position]["descuento_7"].ToString());
				double descuento_8 = Convert.ToDouble(tbl_rpa_t_producto.Rows[position]["descuento_8"].ToString());
				double descuento_9 = Convert.ToDouble(tbl_rpa_t_producto.Rows[position]["descuento_9"].ToString());
				double descuento_10 = Convert.ToDouble(tbl_rpa_t_producto.Rows[position]["descuento_10"].ToString());
				double descuento_11 = Convert.ToDouble(tbl_rpa_t_producto.Rows[position]["descuento_11"].ToString());
				double descuento_12 = Convert.ToDouble(tbl_rpa_t_producto.Rows[position]["descuento_12"].ToString());
				double descuento_13 = Convert.ToDouble(tbl_rpa_t_producto.Rows[position]["descuento_13"].ToString());
				double descuento_14 = Convert.ToDouble(tbl_rpa_t_producto.Rows[position]["descuento_14"].ToString());

				DataTable discount_product_Id = tb_Recordset_MySQL_Plataforma("SELECT * from discount_product where product_id = '" + Convert.ToString(id_plataforma) + "'", "discount_product");
				if (discount_product_Id.Rows.Count == 0)
				{
					 await Insert_Plataforma("discount_product", "created_at = current_timestamp, updated_at = current_timestamp, product_id = '" + Convert.ToString(id_plataforma) + "', level_1 = '" + Convert.ToString(descuento_1) + "', level_2 = '" + Convert.ToString(descuento_2) + "', level_3 = '" + Convert.ToString(descuento_3) + "', level_4 = '" + Convert.ToString(descuento_4) + "', level_5 = '" + Convert.ToString(descuento_5) + "', level_6 = '" + Convert.ToString(descuento_6) + "', level_7 = '" + Convert.ToString(descuento_7) + "', level_8 = '" + Convert.ToString(descuento_8) + "', level_9 = '" + Convert.ToString(descuento_9) + "', level_10 = '" + Convert.ToString(descuento_10) + "', level_11 = '" + Convert.ToString(descuento_11) + "', level_12 = '" + Convert.ToString(descuento_12) + "', level_13 = '" + Convert.ToString(descuento_13) + "', level_14 = '" + Convert.ToString(descuento_14) + "'");
				}
				else
				{
					 await Update_Plataforma("UPDATE discount_product SET updated_at = current_timestamp, level_1 = '" + Convert.ToString(descuento_1) + "', level_2 = '" + Convert.ToString(descuento_2) + "', level_3 = '" + Convert.ToString(descuento_3) + "', level_4 = '" + Convert.ToString(descuento_4) + "', level_5 = '" + Convert.ToString(descuento_5) + "', level_6 = '" + Convert.ToString(descuento_6) + "', level_7 = '" + Convert.ToString(descuento_7) + "', level_8 = '" + Convert.ToString(descuento_8) + "', level_9 = '" + Convert.ToString(descuento_9) + "', level_10 = '" + Convert.ToString(descuento_10) + "', level_11 = '" + Convert.ToString(descuento_11) + "', level_12 = '" + Convert.ToString(descuento_12) + "', level_13 = '" + Convert.ToString(descuento_13) + "', level_14 = '" + Convert.ToString(descuento_14) + "' WHERE product_id = '" + Convert.ToString(id_plataforma) + "'", "discount_product");
				}
			}
			
			ProgressBarX1.Value += 1;
			position++;
		}
		ProgressBarX1.Value = 0;
	}
}
