// histoclin.frmMonitor
using System;
using System.Data;
using System.Threading;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;

private void SincronizarPedidos()
{
	//Discarded unreachable code: IL_02b6
	checked
	{
		try
		{
			DataTable tbl_shopping = tb_Recordset_MySQL_Plataforma("SELECT shopping.client_id, shopping.comment, shopping.shopping_status, shopping.shipping_status_id, shopping.type_shipping, shopping.type_id, shopping.type_billing, shopping.created_at, shopping.folio_local, shopping.id, shopping.sucursal_id, users.credit, users.credit_days, users.discount, users.nivel_ventas FROM shopping INNER JOIN users ON (users.id = shopping.client_id) WHERE shopping.descargado = 0 OR shopping.folio_local = ''", "shopping INNER users pedidos");
			if (tbl_shopping.Rows.Count == 0)
			{
				return;
			}
			int id_pedido = 0;
			ProgressBarX1.Minimum = 0;
			ProgressBarX1.Maximum = tbl_shopping.Rows.Count;
			int tbl_shopping_Total = tbl_shopping.Rows.Count - 1;
			int position = 0;
			while (true)
			{
				if (position > tbl_shopping_Total)
				{
					break;
				}
				DataTable tbl_id_cliente = tb_Recordset_MySQL_local("SELECT id_cliente from rpa_t_cliente WHERE id_plataforma = " + tbl_shopping.Rows[position]["client_id"].ToString() + "", "rpa_t_cliente pedidos");
				if (tbl_id_cliente.Rows.Count == 0)
				{
					Log_DB("rpa_t_cliente el id de cliente de plataforma: " + tbl_shopping.Rows[position]["client_id"].ToString() + " Error al sincronizar pedidos. El cliente aún no ha sido cargado en plataforma, actualice los datos del cliente local", "rpa_t_cliente");
				}
				else
				{
					int id_cliente = Conversions.ToInteger(tbl_id_cliente.Rows[0]["id_cliente"].ToString());
					string estatus = "SIN SURTIR";
					string tipo_venta = "NORMAL";
					string prioridad = "PRIORIDAD 1";
					string origen = "ECOMMERCE";
					string left = tbl_shopping.Rows[position]["type_billing"].ToString();
					int tbl_shopping_Id = Conversions.ToInteger(tbl_shopping.Rows[position]["id"].ToString());
                    
					string tbl_shopping_created_at = "";
					try
					{
						DateTime dateTime = Conversions.ToDate(tbl_shopping.Rows[position]["created_at"].ToString());
						tbl_shopping_created_at = "'" + Strings.Format(dateTime, "yyyy-MM-dd HH:mm:ss") + "'";
					}
					catch (Exception ex)
					{
						tbl_shopping_created_at = "Null";
					}


					DataTable tbl_shopping_products = tb_Recordset_MySQL_Plataforma("SELECT sum(shopping_products.sub_total) from shopping_products INNER JOIN products on (products.id = shopping_products.product_id) WHERE shopping_products.shopping_id = '" + tbl_shopping.Rows[position]["id"].ToString() + "'", "shopping_products pedido subtotal");
                    double total = 0.0;
					try
					{
						if (tbl_shopping_products.Rows.Count > 0)
						{
							try
							{
								total = Conversions.ToDouble(tbl_shopping_products.Rows[0][0].ToString());
							}
							catch (Exception ex3)
							{
								//
							}
						}
					}
					catch (Exception ex5)
					{
						Log_DB("shopping_products no cuenta con productos relacionados al pedido num: " + tbl_shopping.Rows[position]["id"].ToString(), "shopping_products");
                        ProgressBarX1.Value += 1;
				        position++;
                        continue;
					}

					tbl_shopping_products = tb_Recordset_MySQL_Plataforma("SELECT sum(shopping_products.sub_total) from shopping_products INNER JOIN products on (products.id = shopping_products.product_id) WHERE products.iva = 0 and shopping_products.shopping_id = '" + tbl_shopping.Rows[position]["id"].ToString() + "'", "shopping_products pedido subtotal");
					double _products = 0.0;
					try
					{
						_products = Conversions.ToDouble(tbl_shopping_products.Rows[0][0].ToString());
					}
					catch (Exception ex7)
					{
						_products = 0.0;
					}
					
                    tbl_shopping_products = tb_Recordset_MySQL_Plataforma("SELECT sum(shopping_products.sub_total) from shopping_products INNER JOIN products on (products.id = shopping_products.product_id) WHERE products.iva = 0.16 and shopping_products.shopping_id = '" + tbl_shopping.Rows[position]["id"].ToString() + "'", "shopping_products pedido subtotal");
					double _products;
					try
					{iva
						_products = Conversions.ToDouble(tbl_shopping_products.Rows[0][0].ToString());
					}
					catch (Exception ex9)
					{
						_products = 0.0;
					}
					
                    double iva = 0.0;
					iva = _products * 0.16;
					double subtotal = 0.0;
					subtotal = _products + _products;
					total = subtotal + iva;
					total = Math.Round(total, 2);
					subtotal = Math.Round(subtotal, 2);
					iva = Math.Round(iva, 2);

					int credit_days = 0;
					try
					{
						credit_days = Conversions.ToInteger(tbl_shopping.Rows[position]["credit_days"].ToString());
					}
					catch (Exception ex11)
					{
						credit_days = 0;
					}

					string venta_a_credito = "S";
					if (credit_days == 0)
					{
						venta_a_credito = "N";
					}

					string indicaciones = "";
					indicaciones = tbl_shopping.Rows[position]["comment"].ToString();
					try
					{
						id_pedido = Conversions.ToInteger(tbl_shopping.Rows[position]["folio_local"].ToString());
					}
					catch (Exception ex13)
					{
						id_pedido = 0;
					}

					int id_agente_ventas = 0;
					DataTable tbl_t_cliente_Agente_ID = tb_Recordset_MySQL_local("SELECT id_agente_ventas from t_cliente WHERE id_cliente = " + Conversions.ToString(id_cliente) + "", "t_cliente pedidos");
					if (tbl_t_cliente_Agente_ID.Rows.Count > 0)
					{
						id_agente_ventas = Conversions.ToInteger(tbl_t_cliente_Agente_ID.Rows[0]["id_agente_ventas"].ToString());
						if (id_pedido == 0)
						{
							if (Insert_local("t_pedido", "id_cliente = " + Conversions.ToString(id_cliente) + ", id_sucursal = " + Conversions.ToString(SUCURSAL_ID) + ", cancelado = 'S', id_vendedor = " + Conversions.ToString(id_agente_ventas) + ", estatus = '" + estatus + "', tipo_venta = '" + tipo_venta + "', prioridad = '" + prioridad + "', origen = '" + origen + "', subtotal = " + Conversions.ToString(subtotal) + ", iva = " + Conversions.ToString(iva) + ", total = " + Conversions.ToString(total) + ", dias_credito = " + Conversions.ToString(credit_days) + ", fecha = " + tbl_shopping_created_at + ", fecha_origen = " + tbl_shopping_created_at + ", venta_a_credito = '" + venta_a_credito + "', modifico_id_sucursal = " + Conversions.ToString(SUCURSAL_ID) + ", indicaciones = '" + indicaciones + "'"))
							{
								DataTable tbl_t_pedido_LastID = tb_Recordset_MySQL_local("SELECT max(id_pedido) from t_pedido where origen = '" + origen + "' and id_cliente = " + Conversions.ToString(id_cliente) + "", "t_pedido");
								if (tbl_t_pedido_LastID.Rows.Count > 0)
								{
									id_pedido = Conversions.ToInteger(tbl_t_pedido_LastID.Rows[0][0].ToString());
									Update_Plataforma("UPDATE shopping SET folio_local = '" + Conversions.ToString(id_pedido) + "' WHERE id = " + tbl_shopping.Rows[position]["id"].ToString() + "", "shopping");
									Insert_local("rpa_t_pedido", "id_pedido = " + Conversions.ToString(id_pedido) + ", sinc = 0, id_plataforma = " + Conversions.ToString(tbl_shopping_Id) + ", updated_at = current_timestamp");
									
									string facturar_Indicaciones = "";
									if (Operators.CompareString(left, "factura", TextCompare: false) == 0)
									{
										facturar_Indicaciones = "FACTURAR VENTA, " + indicaciones;
										DataTable dataTable6 = tb_Recordset_MySQL_local("SELECT id_pedido FROM t_pedido_indicacion WHERE id_pedido = " + Conversions.ToString(id_pedido) + "", "t_pedido_indicacion");
										if (dataTable6.Rows.Count == 0)
										{
											Insert_local("t_pedido_indicacion", "id_sucursal = " + Conversions.ToString(SUCURSAL_ID) + ", id_pedido = " + Conversions.ToString(id_pedido) + ", indicacion = '" + facturar_Indicaciones + "'");
										}
									}
									else
									{
										facturar_Indicaciones = indicaciones;
										DataTable t_pedido_indicacion_ID = tb_Recordset_MySQL_local("SELECT id_pedido FROM t_pedido_indicacion WHERE id_pedido = " + Conversions.ToString(id_pedido) + "", "t_pedido_indicacion");
										if (t_pedido_indicacion_ID.Rows.Count == 0)
										{
											Insert_local("t_pedido_indicacion", "id_sucursal = " + Conversions.ToString(SUCURSAL_ID) + ", id_pedido = " + Conversions.ToString(id_pedido) + ", indicacion = '" + facturar_Indicaciones + "'");
										}
									}

									DataTable tbl_shopping_products_ID = tb_Recordset_MySQL_Plataforma("SELECT shopping_products.product_id, shopping_products.quantity, shopping_products.unit_cost, shopping_products.sub_total, shopping_products.descargado, shopping_products.created_at, shopping_products.id, products.iva FROM shopping_products INNER JOIN products ON (products.id = shopping_products.product_id) WHERE shopping_id = '" + tbl_shopping.Rows[position]["id"].ToString() + "' and descargado = 0", "shopping_products");
									int tbl_shopping_products_Total = tbl_shopping_products_ID.Rows.Count - 1;
									int position = 0;
									while (true)
									{
										if (position <= tbl_shopping_products_Total)
										{
											int id_producto = 0;
											DataTable tbl_rpa_t_productoID = tb_Recordset_MySQL_local("SELECT id_producto FROM rpa_t_producto WHERE id_plataforma = " + tbl_shopping_products_ID.Rows[position]["product_id"].ToString() + "", "rpa_t_producto pedidos");
											if (tbl_rpa_t_productoID.Rows.Count > 0)
											{
												id_producto = Conversions.ToInteger(tbl_rpa_t_productoID.Rows[0]["id_producto"].ToString());
												DataTable dataTable10 = tb_Recordset_MySQL_local("SELECT costo_promedio, iva, precio_venta_publico, id_producto FROM t_producto WHERE id_producto = " + Conversions.ToString(id_producto) + "", "t_producto pedido");
												if (dataTable10.Rows.Count == 0)
												{
													Log_DB("id_producto = " + Conversions.ToString(id_producto) + ", Error en SELECT costo_promedio, iva, precio_venta_publico, id_producto FROM t_producto WHERE id_producto =", "t_pedido");
													break;
												}
												
												string shopping_products_created_at_Str = "";
												try
												{
													DateTime shopping_products_created_at = Conversions.ToDate(tbl_shopping_products_ID.Rows[position]["created_at"].ToString());
													shopping_products_created_at_Str = "'" + Strings.Format(shopping_products_created_at, "yyyy-MM-dd") + "'";
												}
												catch (Exception ex15)
												{
													shopping_products_created_at_Str = "Null";
												}
												double shopping_products_quantity = 0.0;
												try
												{
													shopping_products_quantity = Conversions.ToDouble(tbl_shopping_products_ID.Rows[position]["quantity"].ToString());
												}
												catch (Exception ex17)
												{
												}
												double costo_promedio = 0.0;
												try
												{
													costo_promedio = Conversions.ToDouble(dataTable10.Rows[0]["costo_promedio"].ToString());
												}
												catch (Exception ex19)
												{
												}
												double iva = 0.0;
												try
												{
													iva = Conversions.ToDouble(dataTable10.Rows[0]["iva"].ToString());
												}
												catch (Exception ex21)
												{
												}
												double unit_cost = 0.0;
												try
												{
													unit_cost = Conversions.ToDouble(tbl_shopping_products_ID.Rows[position]["unit_cost"].ToString());
													unit_cost *= 1.0 + iva;
												}
												catch (Exception ex23)
												{
												}
												double precio_venta_publico = 0.0;
												try
												{
													precio_venta_publico = Conversions.ToDouble(dataTable10.Rows[0]["precio_venta_publico"].ToString());
												}
												catch (Exception ex25)
												{
												}
												string esquema_de_venta = "";
												esquema_de_venta = "L";
												double descuento = 0.0;
												if (precio_venta_publico > 0.0)
												{
													descuento = (1.0 - unit_cost / precio_venta_publico) * 100.0;
													descuento = Math.Round(descuento, 2);
												}
												unit_cost = Math.Round(unit_cost, 2);
												if (id_pedido == 0)
												{
													DataTable dataTable11 = tb_Recordset_MySQL_local("SELECT id_pedido FROM rpa_t_pedido WHERE id_plataforma = " + tbl_shopping.Rows[position]["id"].ToString() + "", "seguimiento pedidos");
													if (dataTable11.Rows.Count > 0)
													{
														id_pedido = Conversions.ToInteger(dataTable11.Rows[0]["id_pedido"].ToString());
													}
												}
												if (id_pedido > 0 && Insert_local("t_pedido_detalle", "id_sucursal = " + Conversions.ToString(SUCURSAL_ID) + ", id_pedido = " + Conversions.ToString(id_pedido) + ", id_producto = " + Conversions.ToString(id_producto) + ", fecha = " + shopping_products_created_at_Str + ", cantidad = " + Conversions.ToString((int)Math.Round(shopping_products_quantity)) + ", pvp = " + Conversions.ToString(Math.Round(precio_venta_publico, 2)) + ", esquema_de_venta = '" + esquema_de_venta + "', descuento = " + Conversions.ToString(Math.Round(descuento, 2)) + ", costo_unitario = " + Conversions.ToString(Math.Round(costo_promedio, 2)) + ", precio_unitario = " + Conversions.ToString(Math.Round(unit_cost, 2)) + ", porcentaje_iva = " + Conversions.ToString(Math.Round(iva, 2)) + ""))
												{
													Update_Plataforma("UPDATE shopping_products SET descargado = 1 where id = '" + tbl_shopping_products_ID.Rows[position]["id"].ToString() + "'", "shopping_products pedidos");
												}
											}
											position++;
											continue;
										}
										DataTable tbl_t_pedido_detalle_id = tb_Recordset_MySQL_local("SELECT SUM(cantidad * precio_unitario) FROM t_pedido_detalle WHERE id_pedido = " + Conversions.ToString(id_pedido) + "", "t_pedido_detalle 102");
										double pedido_detalle = 0.0;
										if (Operators.CompareString(tbl_t_pedido_detalle_id.Rows[0][0].ToString(), "", TextCompare: false) != 0)
										{
											pedido_detalle = Conversions.ToDouble(tbl_t_pedido_detalle_id.Rows[0][0].ToString());
										}
										tbl_t_pedido_detalle_id = tb_Recordset_MySQL_local("SELECT SUM(cantidad * (precio_unitario/1.16)) * 0.16 FROM t_pedido_detalle WHERE id_pedido = " + Conversions.ToString(id_pedido) + " AND porcentaje_iva = 0.16", "t_pedido_detalle 103");
										double pedido_detalle_2 = 0.0;
										if (tbl_t_pedido_detalle_id.Rows.Count > 0)
										{
											try
											{
												if (Operators.CompareString(tbl_t_pedido_detalle_id.Rows[0][0].ToString(), "", TextCompare: false) != 0)
												{
													pedido_detalle_2 = Conversions.ToDouble(tbl_t_pedido_detalle_id.Rows[0][0].ToString());
												}
											}
											catch (Exception ex27)
											{
												
											}
										}
										double __Subtotal = pedido_detalle - pedido_detalle_2;
										Update_local("UPDATE t_pedido SET subtotal = " + Conversions.ToString(Math.Round(__Subtotal, 2)) + ", iva = " + Conversions.ToString(Math.Round(pedido_detalle_2, 2)) + ", total = " + Conversions.ToString(Math.Round(pedido_detalle, 2)) + " WHERE id_pedido = " + Conversions.ToString(id_pedido) + "", "t_pedido");
										Thread.Sleep(3000);
										DataTable dataTable13 = tb_Recordset_MySQL_Plataforma("SELECT id from shopping_products WHERE shopping_id = '" + tbl_shopping.Rows[position]["id"].ToString() + "' and descargado = 0", "shopping_products");
										if (dataTable13.Rows.Count == 0 && Update_Plataforma("UPDATE shopping SET descargado = 1 where id = '" + tbl_shopping.Rows[position]["id"].ToString() + "'", "shopping pedidos") && !Update_local("UPDATE t_pedido SET cancelado = 'N', id_sucursal = " + Conversions.ToString(SUCURSAL_ID) + " WHERE id_pedido = " + Conversions.ToString(id_pedido) + "", "t_pedido"))
										{
											Update_Plataforma("UPDATE shopping SET descargado = 0 where id = '" + tbl_shopping.Rows[position]["id"].ToString() + "'", "shopping pedidos");
										}
										break;
									}
								}
								else
								{
									Log_DB("Error en Select max(id_pedido) from t_pedido where origen =", "t_pedido");
								}
							}
						}
						else if (id_pedido != 0)
						{
							DataTable tbl_id_pedido = tb_Recordset_MySQL_local("SELECT id_pedido from rpa_t_pedido WHERE id_pedido = " + Conversions.ToString(id_pedido) + "", "rpa_t_pedido");
							if (tbl_id_pedido.Rows.Count == 0)
							{
								Insert_local("rpa_t_pedido", "id_pedido = " + Conversions.ToString(id_pedido) + ", sinc = 0, id_plataforma = " + Conversions.ToString(tbl_shopping_Id) + ", updated_at = current_timestamp");
							}
							DataTable tbl_shopping_products = tb_Recordset_MySQL_Plataforma("SELECT shopping_products.product_id, shopping_products.quantity, shopping_products.unit_cost, shopping_products.sub_total, shopping_products.descargado, shopping_products.created_at, shopping_products.id, products.iva FROM shopping_products INNER JOIN products ON (products.id = shopping_products.product_id) WHERE shopping_id = '" + tbl_shopping.Rows[position]["id"].ToString() + "' and descargado = 0", "shopping_products");
							int shopping_products_Total = tbl_shopping_products.Rows.Count - 1;
							int position = 0;
							while (true)
							{
								if (position <= shopping_products_Total)
								{
									int num28 = 0;
									DataTable dataTable16 = tb_Recordset_MySQL_local("SELECT id_producto from rpa_t_producto WHERE id_plataforma = " + tbl_shopping_products.Rows[num26]["product_id"].ToString() + "", "rpa_t_producto pedidos");
									if (dataTable16.Rows.Count > 0)
									{
										num28 = Conversions.ToInteger(dataTable16.Rows[0]["id_producto"].ToString());
										DataTable dataTable17 = tb_Recordset_MySQL_local("SELECT costo_promedio, iva, precio_venta_publico FROM t_producto WHERE id_producto = " + Conversions.ToString(num28) + "", "t_producto pedido");
										if (dataTable17.Rows.Count == 0)
										{
											Log_DB("id_producto = " + Conversions.ToString(num28) + ", Error en SELECT costo_promedio, iva, precio_venta_publico, id_producto FROM t_producto WHERE id_producto = //cuando id_pedido > 0//", "t_pedido");
											break;
										}
										string text11 = "";
										try
										{
											DateTime dateTime3 = Conversions.ToDate(tbl_shopping_products.Rows[num26]["created_at"].ToString());
											text11 = "'" + Strings.Format(dateTime3, "yyyy-MM-dd") + "'";
										}
										catch (Exception ex29)
										{
											ProjectData.SetProjectError(ex29);
											Exception ex30 = ex29;
											text11 = "current_tiemstamp";
											ProjectData.ClearProjectError();
										}
										double a2 = 0.0;
										try
										{
											a2 = Conversions.ToDouble(tbl_shopping_products.Rows[num26]["quantity"].ToString());
										}
										catch (Exception ex31)
										{
											ProjectData.SetProjectError(ex31);
											Exception ex32 = ex31;
											ProjectData.ClearProjectError();
										}
										double value3 = 0.0;
										try
										{
											value3 = Conversions.ToDouble(dataTable17.Rows[0]["costo_promedio"].ToString());
										}
										catch (Exception ex33)
										{
											ProjectData.SetProjectError(ex33);
											Exception ex34 = ex33;
											ProjectData.ClearProjectError();
										}
										double num29 = 0.0;
										try
										{
											num29 = Conversions.ToDouble(dataTable17.Rows[0]["iva"].ToString());
										}
										catch (Exception ex35)
										{
											ProjectData.SetProjectError(ex35);
											Exception ex36 = ex35;
											ProjectData.ClearProjectError();
										}
										double num30 = 0.0;
										try
										{
											num30 = Conversions.ToDouble(tbl_shopping_products.Rows[num26]["unit_cost"].ToString());
											num30 *= 1.0 + num29;
										}
										catch (Exception ex37)
										{
											ProjectData.SetProjectError(ex37);
											Exception ex38 = ex37;
											ProjectData.ClearProjectError();
										}
										double num31 = 0.0;
										try
										{
											num31 = Conversions.ToDouble(dataTable17.Rows[0]["precio_venta_publico"].ToString());
										}
										catch (Exception ex39)
										{
											ProjectData.SetProjectError(ex39);
											Exception ex40 = ex39;
											ProjectData.ClearProjectError();
										}
										string text12 = "";
										text12 = "L";
										double value4 = 0.0;
										if (num31 > 0.0)
										{
											value4 = (1.0 - num30 / num31) * 100.0;
											value4 = Math.Round(value4, 2);
										}
										num30 = Math.Round(num30, 2);
										if (id_pedido > 0 && Insert_local("t_pedido_detalle", "id_sucursal = " + Conversions.ToString(SUCURSAL_ID) + ", id_pedido = " + Conversions.ToString(id_pedido) + ", id_producto = " + Conversions.ToString(num28) + ", fecha = " + text11 + ", cantidad = " + Conversions.ToString((int)Math.Round(a2)) + ", pvp = " + Conversions.ToString(Math.Round(num31, 2)) + ", esquema_de_venta = '" + text12 + "', descuento = " + Conversions.ToString(Math.Round(value4, 2)) + ", costo_unitario = " + Conversions.ToString(Math.Round(value3, 2)) + ", precio_unitario = " + Conversions.ToString(Math.Round(num30, 2)) + ", porcentaje_iva = " + Conversions.ToString(Math.Round(num29, 2)) + ""))
										{
											Update_Plataforma("UPDATE shopping_products SET descargado = 1 where id = '" + tbl_shopping_products.Rows[num26]["id"].ToString() + "'", "shopping_products pedidos");
										}
									}
									num26++;
									continue;
								}
								DataTable dataTable18 = tb_Recordset_MySQL_local("SELECT SUM(cantidad * precio_unitario) from t_pedido_detalle WHERE id_pedido = " + Conversions.ToString(id_pedido) + "", "t_pedido_detalle 100");
								double num32 = 0.0;
								if (Operators.CompareString(dataTable18.Rows[0][0].ToString(), "", TextCompare: false) != 0)
								{
									num32 = Conversions.ToDouble(dataTable18.Rows[0][0].ToString());
								}
								dataTable18 = tb_Recordset_MySQL_local("SELECT SUM(cantidad * (precio_unitario/1.16)) * 0.16 from t_pedido_detalle WHERE id_pedido = " + Conversions.ToString(id_pedido) + " AND porcentaje_iva = 0.16", "t_pedido_detalle 101");
								double num33 = 0.0;
								if (dataTable18.Rows.Count > 0)
								{
									try
									{
										if (Operators.CompareString(dataTable18.Rows[0][0].ToString(), "", TextCompare: false) != 0)
										{
											num33 = Conversions.ToDouble(dataTable18.Rows[0][0].ToString());
										}
									}
									catch (Exception ex41)
									{
										ProjectData.SetProjectError(ex41);
										Exception ex42 = ex41;
										ProjectData.ClearProjectError();
									}
								}
								double num34 = 0.0;
								num34 = num32 - num33;
								Update_local("UPDATE t_pedido SET subtotal = " + Conversions.ToString(Math.Round(num34, 2)) + ", iva = " + Conversions.ToString(Math.Round(num33, 2)) + ", total = " + Conversions.ToString(Math.Round(num32, 2)) + " WHERE id_pedido = " + Conversions.ToString(id_pedido) + "", "t_pedido");
								Thread.Sleep(3000);
								DataTable tbl_shopping_products = tb_Recordset_MySQL_Plataforma("SELECT id from shopping_products WHERE shopping_id = '" + tbl_shopping.Rows[position]["id"].ToString() + "' and descargado = 0", "shopping_products");
								if (tbl_shopping_products.Rows.Count == 0 && Update_Plataforma("UPDATE shopping SET descargado = 1 where id = '" + tbl_shopping.Rows[position]["id"].ToString() + "'", "shopping pedidos") && !Update_local("UPDATE t_pedido SET cancelado = 'N', id_sucursal = " + Conversions.ToString(SUCURSAL_ID) + " WHERE id_pedido = " + Conversions.ToString(id_pedido) + "", "t_pedido"))
								{
									Update_Plataforma("UPDATE shopping SET descargado = 0 where id = '" + tbl_shopping.Rows[position]["id"].ToString() + "'", "shopping pedidos");
								}
								break;
							}
						}
					}
					else
					{
						Log_DB("Error en Select id_agente_ventas from t_cliente WHERE id_cliente =", "t_cliente");
					}
				}
				//goto IL_1a5c;
				//IL_1a5c:
				ProgressBarX1.Value += 1;
				position++;
			}
			ProgressBarX1.Value = 0;
		}
		catch (Exception ex43)
		{			
			Log_DB(ex44.Message, "CRITICAL");
		}
	}
}
