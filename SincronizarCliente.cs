//
// histoclin.frmMonitor
using System.Data;
using Microsoft.VisualBasic.CompilerServices;

private void SincronizarCliente()
{
	DataTable tbl_cliente = await tb_Recordset_MySQL_local("SELECT t_cliente.correo_electronico, t_cliente.usuario_ecommerce, t_cliente.correo_electronico_adicional, t_cliente.tiene_credito, t_cliente.limite_credito, t_cliente.dias_credito, t_cliente.nivel_descuento_lista, t_cliente.descuento_lista, t_cliente.telefono, t_cliente.telefono_2, t_cliente.rfc, t_cliente.nombre, t_cliente.razon_social, t_cliente.calle, t_cliente.numero_exterior, t_cliente.numero_interior, t_cliente.colonia, t_cliente.codigo_postal, t_cliente.ciudad, t_estado.nombre as estado, t_cliente.direccion_referencia, t_cliente.entrega_calle, t_cliente.entrega_numero_exterior, t_cliente.entrega_numero_interior, t_cliente.entrega_colonia, t_cliente.entrega_codigo_postal, t_cliente.entrega_ciudad, t_cliente.nivel_ventas, t_estado_entrega.nombre as estado_entrega, t_cliente.activo, t_cliente.id_agente_ventas, rpa_t_cliente.id, rpa_t_cliente.id_cliente, rpa_t_cliente.id_plataforma FROM rpa_t_cliente INNER JOIN t_cliente ON (t_cliente.id_cliente = rpa_t_cliente.id_cliente) LEFT JOIN t_estado ON (t_estado.id_estado = t_cliente.id_estado) LEFT JOIN t_estado t_estado_entrega ON (t_estado_entrega.id_estado = t_cliente.id_estado_entrega) WHERE rpa_t_cliente.sinc = 1", "rpa_t_cliente INNER t_cliente");
	if (tbl_cliente.Rows.Count == 0)
	{
		return;
	}
	int id_plataforma = 0;
	ProgressBarX1.Minimum = 0;
	ProgressBarX1.Maximum = tbl_cliente.Rows.Count;
	checked
	{
		int cliente_Total = tbl_cliente.Rows.Count - 1;
		int position = 0;
		while (true)
		{
			if (position > cliente_Total)
			{
				break;
			}
			id_plataforma = Convert.ToInt32(tbl_cliente.Rows[position]["id_plataforma"].ToString());
			string password = "$2y$10$ZttKb5Gc3zwmm6P6XVIrbObIQijDi/mVUnsBos/GUK7N10sy5e9pK";
			//string correo_electronico = "";
			string correo_electronico = tbl_cliente.Rows[position]["correo_electronico"].ToString();
			//string usuario_ecommerce = "";
			string usuario_ecommerce = tbl_cliente.Rows[position]["usuario_ecommerce"].ToString();
			if (String.Compare(tbl_cliente.Rows[position]["usuario_ecommerce"].ToString(), "",  false) == 0)
			{
				usuario_ecommerce = tbl_cliente.Rows[position]["id_cliente"].ToString() + "@client";
			}
			//double limite_credito = 0.0;
			double limite_credito = Convert.ToDouble(tbl_cliente.Rows[position]["limite_credito"].ToString());
			string virify = "1";
			//string discount = "";
			string discount = nivel_descuento(Convert.ToInt32(tbl_cliente.Rows[position]["nivel_descuento_lista"].ToString()));
			string role = "Cliente";
			//int dias_credito = 0;
			int dias_credito = Convert.ToInt32(tbl_cliente.Rows[position]["dias_credito"].ToString());
			string code = "";
			int activo = ((String.Compare(tbl_cliente.Rows[position]["activo"].ToString(), "S",  false) == 0) ? 1 : 0);
			string nivel_ventas = tbl_cliente.Rows[position]["nivel_ventas"].ToString();
			int id_agente_ventas = Convert.ToInt32(tbl_cliente.Rows[position]["id_agente_ventas"].ToString());
            //!!! num10 
			//id_plataforma = Convert.ToInt32(tbl_cliente.Rows[position]["id_plataforma"].ToString());
			string clientNombre = tbl_cliente.Rows[position]["nombre"].ToString();
			string clientLastName = "";
			string client_entrega_calle = tbl_cliente.Rows[position]["entrega_calle"].ToString();
			string client_entrega_numero_exterior = tbl_cliente.Rows[position]["entrega_numero_exterior"].ToString();
			string client_entrega_numero_interior = tbl_cliente.Rows[position]["entrega_numero_interior"].ToString();
			string client_entrega_codigo_postal = tbl_cliente.Rows[position]["entrega_codigo_postal"].ToString();
			string client_entrega_colonia = tbl_cliente.Rows[position]["entrega_colonia"].ToString();
			string client_entrega_ciudad = tbl_cliente.Rows[position]["entrega_ciudad"].ToString();
			string client_entrega_estado = tbl_cliente.Rows[position]["estado_entrega"].ToString();
			string client_telefono = tbl_cliente.Rows[position]["telefono"].ToString() + ", " + tbl_cliente.Rows[position]["telefono_2"].ToString();
			string client_predeterminada = "1";
            //*** no se usan
			string client_rfc = tbl_cliente.Rows[position]["rfc"].ToString();
			string client_razon_social = tbl_cliente.Rows[position]["razon_social"].ToString();
            //--------------

			string client_calle = tbl_cliente.Rows[position]["calle"].ToString();
			if (String.Compare(client_entrega_calle.Trim(), "",  false) == 0)
			{
				client_entrega_calle = client_calle;
			}

			string client_numero_exterior = tbl_cliente.Rows[position]["numero_exterior"].ToString();
			if (String.Compare(client_entrega_numero_exterior.Trim(), "",  false) == 0)
			{
				client_entrega_numero_exterior = client_numero_exterior;
			}

			string client_numero_interior = tbl_cliente.Rows[position]["numero_interior"].ToString();
			if (String.Compare(client_entrega_numero_interior.Trim(), "",  false) == 0)
			{
				client_entrega_numero_interior = client_numero_interior;
			}

			string client_codigo_postal = tbl_cliente.Rows[position]["codigo_postal"].ToString();
			if (String.Compare(client_entrega_codigo_postal.Trim(), "",  false) == 0)
			{
				client_entrega_codigo_postal = client_codigo_postal;
			}

			string client_colonia = tbl_cliente.Rows[position]["colonia"].ToString();
			if (String.Compare(client_entrega_colonia.Trim(), "",  false) == 0)
			{
				client_entrega_colonia = client_colonia;
			}
			
			string client_ciudad = tbl_cliente.Rows[position]["ciudad"].ToString();
			if (String.Compare(client_entrega_ciudad.Trim(), "",  false) == 0)
			{
				client_entrega_ciudad = client_ciudad;
			}

			string client_estado = tbl_cliente.Rows[position]["estado"].ToString();
			if (String.Compare(client_entrega_estado.Trim(), "",  false) == 0)
			{
				client_entrega_estado = client_estado;
			}


			string client_country = "187";

			if (id_plataforma == 0)
			{
				DataTable tbl_users = await tb_Recordset_MySQL_Plataforma("SELECT id from users where email = '" + usuario_ecommerce + "'", "users");
				if (tbl_users.Rows.Count <= 0)
				{
					if (await Insert_Plataforma("users", "created_at = current_timestamp, updated_at = current_timestamp, sucursal_id = '" + Convert.ToString(SUCURSAL_ID) + "', password = '" + password + "',email = '" + usuario_ecommerce + "', credit = '" + Convert.ToString(limite_credito) + "', activated = '" + Convert.ToString(activo) + "', verified = '" + virify + "', discount = '" + discount + "', nivel_ventas = '" + nivel_ventas + "', id_agente_ventas = '" + Convert.ToString(id_agente_ventas) + "', role = '" + role + "', code = '" + code + "', credit_days = '" + Convert.ToString(dias_credito) + "'"))
					{
						DataTable tbl_users_lastId = await tb_Recordset_MySQL_Plataforma("SELECT max(id) from users", "users");
						id_plataforma = Convert.ToInt32(tbl_users_lastId.Rows[0][0].ToString());
						await Update_local("UPDATE rpa_t_cliente SET sinc = 0, updated_at_plat = current_timestamp, id_plataforma = '" + Convert.ToString(id_plataforma) + "' WHERE id = " + tbl_cliente.Rows[position]["id"].ToString(), "rpa_t_cliente");
						await Insert_Plataforma("address", "created_at = current_timestamp, updated_at = current_timestamp, user_id = '" + Convert.ToString(id_plataforma) + "',name = '" + clientNombre + "', lastname = '" + clientLastName + "', address = '" + client_entrega_calle + "', no_ext = '" + client_entrega_numero_exterior + "', no_int = '" + client_entrega_numero_interior + "', postal = '" + client_entrega_codigo_postal + "', colony = '" + client_entrega_colonia + "', city = '" + client_entrega_ciudad + "', state = '" + client_entrega_estado + "', phone = '" + client_telefono + "', predeterminada = '" + client_predeterminada + "'");
						await Insert_Plataforma("billing_address", "created_at = current_timestamp, updated_at = current_timestamp, user_id = '" + Convert.ToString(id_plataforma) + "',name = '" + clientNombre + "', lastname = '" + clientLastName + "', address = '" + client_calle + "', no_ext = '" + client_numero_exterior + "', no_int = '" + client_numero_interior + "', postal = '" + client_codigo_postal + "', colony = '" + client_colonia + "', city = '" + client_ciudad + "', state = '" + client_estado + "', phone = '" + client_telefono + "', predeterminada = '" + client_predeterminada + "'");
						await Insert_Plataforma("profiles", "created_at = current_timestamp, updated_at = current_timestamp, user_id = '" + Convert.ToString(id_plataforma) + "',name = '" + clientNombre + "', lastname = '" + clientLastName + "', name_company = '" + client_razon_social + "', email_contact = '" + correo_electronico + "', phone = '" + client_telefono + "', movil = '" + client_telefono + "', street = '" + client_entrega_calle + "', nro_ext = '" + client_entrega_numero_exterior + "', nro_int = '" + client_entrega_numero_interior + "', colony = '" + client_entrega_colonia + "', municipality = '" + client_entrega_ciudad + "', federal_entity = '" + client_entrega_estado + "', postal = '" + client_entrega_codigo_postal + "', country = '" + client_country + "', city = '" + client_entrega_ciudad + "', state = '" + client_entrega_estado + "'");
					}
					//goto IL_13c1;
                    ProgressBarX1.Value += 1;
			        position++;
                    continue;
				}
				id_plataforma = Convert.ToInt32(tbl_users.Rows[0][0].ToString());
				id_plataforma = Convert.ToInt32(tbl_users.Rows[0][0].ToString());
				await Update_local("UPDATE rpa_t_cliente SET updated_at_plat = current_timestamp, id_plataforma = '" + Convert.ToString(id_plataforma) + "' WHERE id = " + tbl_cliente.Rows[position]["id"].ToString(), "rpa_t_cliente");
			}
			if (Update_Plataforma("UPDATE users SET updated_at = current_timestamp, sucursal_id = '" + Convert.ToString(SUCURSAL_ID) + "', email = '" + usuario_ecommerce + "', credit = '" + Convert.ToString(limite_credito) + "', activated = '" + Convert.ToString(activo) + "', verified = '" + virify + "', discount = '" + discount + "', nivel_ventas = '" + nivel_ventas + "', id_agente_ventas = '" + Convert.ToString(id_agente_ventas) + "', role = '" + role + "', code = '" + code + "', credit_days = '" + Convert.ToString(dias_credito) + "' WHERE id = '" + Convert.ToString(id_plataforma) + "'", "users"))
			{
				await Update_local("UPDATE rpa_t_cliente SET updated_at_plat = current_timestamp, sinc = 0 WHERE id = " + tbl_cliente.Rows[position]["id"].ToString(), "rpa_t_cliente");
				await Update_Plataforma("UPDATE address SET updated_at = current_timestamp, name = '" + clientNombre + "', lastname = '" + clientLastName + "', address = '" + client_entrega_calle + "', no_ext = '" + client_entrega_numero_exterior + "', no_int = '" + client_entrega_numero_interior + "', postal = '" + client_entrega_codigo_postal + "', colony = '" + client_entrega_colonia + "', city = '" + client_entrega_ciudad + "', state = '" + client_entrega_estado + "', phone = '" + client_telefono + "', predeterminada = '" + client_predeterminada + "' WHERE user_id = '" + Convert.ToString(id_plataforma) + "'", "address");
				await Update_Plataforma("UPDATE billing_address SET updated_at = current_timestamp, user_id = '" + Convert.ToString(id_plataforma) + "',name = '" + clientNombre + "', lastname = '" + clientLastName + "', address = '" + client_calle + "', no_ext = '" + client_numero_exterior + "', no_int = '" + client_numero_interior + "', postal = '" + client_codigo_postal + "', colony = '" + client_colonia + "', city = '" + client_ciudad + "', state = '" + client_estado + "', phone = '" + client_telefono + "', predeterminada = '" + client_predeterminada + "' WHERE user_id = '" + Convert.ToString(id_plataforma) + "'", "billing_address");
				DataTable tbl_profiles = tb_Recordset_MySQL_Plataforma("SELECT * from profiles where user_id = '" + Convert.ToString(id_plataforma) + "'", "profiles");
				if (tbl_profiles.Rows.Count == 0)
				{
					await Insert_Plataforma("profiles", "created_at = current_timestamp, updated_at = current_timestamp, user_id = '" + Convert.ToString(id_plataforma) + "',name = '" + clientNombre + "', lastname = '" + clientLastName + "', name_company = '" + client_razon_social + "', email_contact = '" + correo_electronico + "', phone = '" + client_telefono + "', movil = '" + client_telefono + "', street = '" + client_entrega_calle + "', nro_ext = '" + client_entrega_numero_exterior + "', nro_int = '" + client_entrega_numero_interior + "', colony = '" + client_entrega_colonia + "', municipality = '" + client_entrega_ciudad + "', federal_entity = '" + client_entrega_estado + "', postal = '" + client_entrega_codigo_postal + "', country = '" + client_country + "', city = '" + client_entrega_ciudad + "', state = '" + client_entrega_estado + "'");
				}
				else
				{
					await Update_Plataforma("UPDATE profiles SET updated_at = current_timestamp, name = '" + clientNombre + "', lastname = '" + clientLastName + "', name_company = '" + client_razon_social + "', email_contact = '" + correo_electronico + "', phone = '" + client_telefono + "', movil = '" + client_telefono + "', street = '" + client_entrega_calle + "', nro_ext = '" + client_entrega_numero_exterior + "', nro_int = '" + client_entrega_numero_interior + "', colony = '" + client_entrega_colonia + "', municipality = '" + client_entrega_ciudad + "', federal_entity = '" + client_entrega_estado + "', postal = '" + client_entrega_codigo_postal + "', country = '" + client_country + "', city = '" + client_entrega_ciudad + "', state = '" + client_entrega_estado + "' WHERE user_id = '" + Convert.ToString(id_plataforma) + "'", "profiles");
				}
			}

			ProgressBarX1.Value += 1;
			position++;
		}
		ProgressBarX1.Value = 0;
	}
}
