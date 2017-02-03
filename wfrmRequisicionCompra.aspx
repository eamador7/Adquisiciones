<%@ Page Language="VB" MasterPageFile="MPCompras.master"  AutoEventWireup="false" CodeFile="wfrmRequisicionCompra.aspx.vb" Inherits="wfrmRequisicionCompra" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
<%--	<link rel="stylesheet" href="http://localhost:1741/code.jquery.com/ui/1.11.4/themes/smoothness/jquery-ui.css"/>
	<script src="//code.jquery.com/jquery-1.10.2.js"></script>
	<script src="//code.jquery.com/ui/1.11.4/jquery-ui.js"></script>--%>

	<style>
		.tbl-txt {
			font-size:.7em;
		}
	</style>
</asp:Content>
	
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">   
	<asp:HiddenField ID="hdnRequisicionCompra" runat="server" />
	<asp:HiddenField ID="hdnPrecio" runat="server" />
	<asp:HiddenField ID="hdnCco_id" runat="server" />
	<asp:HiddenField ID="hdnCco_numero" runat="server" />
	<asp:HiddenField ID="hdnCodigo" runat="server" />
	<asp:HiddenField ID="hdnExistencia" runat="server" />
	<asp:HiddenField ID="hdnConcepto" runat="server" />  
	<asp:HiddenField ID="hdnMotivoCambio" runat="server" />
	<asp:HiddenField ID="hdnCantidad" runat="server" />
	<asp:HiddenField ID="hdnContrato" runat="server" />
	<asp:HiddenField ID="hdnProveedor" runat="server" />
	<asp:HiddenField ID="hdnMontoContrato" runat="server" />

	<asp:Timer ID="Timer1" runat="server" Enabled="false" Interval="2000"></asp:Timer>
	<div class="panel panel-success">
		<div class="panel-body">
			<div class="row">
<%--                <div class="form-group col-lg-2 col-md-2 col-sm-2"> 
					<asp:label ID="lblFolio" runat="server" ForeColor="Red">FOLIO:</asp:label>               
					<asp:TextBox ID="txtFolio"  ReadOnly="true" class="form-control" runat="server" ToolTip="Folio"></asp:TextBox>                
				</div> --%>
			  
				<div class="form-group col-lg-2 col-md-2 col-sm-2"> 
					<asp:label ID="Label4" runat="server" >NO. EMPLEADO:</asp:label>               
					<asp:TextBox ID="txtnoEmpleado"  ReadOnly="true" class="form-control" runat="server" ToolTip="Folio"></asp:TextBox>                
				</div>
				<div class="form-group col-lg-5 col-md-5 col-sm-5">                
					<asp:label ID="lblNombre" runat="server">NOMBRE:</asp:label>
					<asp:TextBox ID="txtNombre" ReadOnly="true" class="form-control" runat="server" ToolTip="Nombre" placeholder="Nombre"></asp:TextBox>
				</div>  
		   
				  <div class="form-group col-lg-2 col-md-2 col-sm-2">                
					<asp:label ID="lblFecha" runat="server">FECHA:</asp:label>               
					<asp:TextBox ID="txtFecha" ReadOnly="true" class="form-control" runat="server" ToolTip="Fecha" placeholder="Fecha"></asp:TextBox>                 
				</div>                                                      
			</div>
		
							
		  <div class="row">                    
				 <div class="form-group col-lg-9 col-md-9 col-sm-9">
					<asp:label ID="Label1" runat="server">ESPECIFICACIONES TÉCNICAS:</asp:label>  
					<asp:TextBox ID="txtObservaciones" TextMode="MultiLine" class="form-control" runat="server" ToolTip="Especificaciones Técnicas" placeholder="Especificaciones técnicas (Obligatorias)"></asp:TextBox>                
				</div>                
			</div>  
			
		</div>
	</div>
    <asp:Panel ID="searchPanel" runat="server"  Visible="true" DefaultButton="btnBuscarProducto">
	<div class="panel panel-success">
		<div class="row panel-body" style="color:black;padding-bottom: 0;">
			<div class="form-group col-lg-2 col-md-2 col-sm-2">                
					<asp:TextBox ID="txtCodigo" runat="server" class="form-control" ToolTip="Codigo" placeholder="Codigo"></asp:TextBox>                                           
			</div>
			<div class="form-group col-lg-5 col-md-5 col-sm-5" style="color:rgba(250, 184, 28, 0.50)">
				<asp:TextBox ID="txtDescripcion" runat="server" class="form-control" ToolTip="Descripción" placeholder="Descripción"></asp:TextBox>
			</div>   
			<div class="form-group col-lg-1 col-md-1 col-sm-1"> 
				<asp:LinkButton ID="btnBuscarProducto" runat="server" Text="Buscar" class="btn btn-default" />
			</div>                      
			<div class="form-group col-lg-2 col-md-2 col-sm-2">
				<asp:TextBox ID="txtCantidad"  TextMode="Number"  runat="server" class="form-control" style="text-align:right" ToolTip="Cantidad" placeholder="Cantidad"></asp:TextBox>                                          
			</div>      
            <div class="form-group col-lg-1 col-md-1 col-sm-1">
				<asp:TextBox ID="txtUDM"  readonly="true"  runat="server" class="form-control" style="text-align:right" ToolTip="UDM" placeholder="UDM"></asp:TextBox>                                          
			</div>                                                          
		</div>
		<div class="row panel-body" style="color:black;padding-top: 0px;">
			<div class="form-group col-lg-2 col-md-2 col-sm-2">
			</div>
				   
		  <div class="form-group col-lg-5 col-md-5 col-sm-5"> 
				<asp:TextBox ID="txtAdicional"  TextMode="MultiLine"  runat="server" class="form-control" ToolTip="Descripcion adicional" placeholder="Descripcion adicional del articulo"></asp:TextBox>
			</div> 
			<div class="form-group col-lg-1 col-md-1 col-sm-1"> 
				<asp:LinkButton ID="btnLimpiar" runat="server" Text="Limpiar" class="btn btn-default" />
			</div>
			<div class="form-group col-lg-2 col-md-2 col-sm-2"> 
				<asp:LinkButton  ID="btnAgregarProducto" runat="server" Text="Agregar Articulo" class="btn btn-default"  />
			</div>
		</div>
        </div>
        </asp:Panel>
		<div class="row" >
			<asp:Panel ID="pnlArticulos" runat="server" class="panel-body" Visible="false">
				<div class="col-md-1">
				</div>
				<div class="col-md-10">
					  <h4>CATÁLOGO DE ARTÍCULOS</h4>
					<asp:GridView ID="grvArticulos" Width="80%" class="table tbl-sel" runat="server" AutoGenerateColumns="false">   
						<HeaderStyle HorizontalAlign="Center" />                 
					   <RowStyle HorizontalAlign="center"/>                              
						<Columns>
						<asp:BoundField DataField="codigo" HeaderText="Codigo"/>
						<asp:BoundField DataField="concepto" HeaderText="Concepto"/>                        
						<asp:BoundField DataField="udm" HeaderText="UDM"/>    
						<asp:BoundField DataField="precio" HeaderText="PRECIO" Visible="false"/>
						<asp:BoundField DataField="cco_id" HeaderText="CCO_ID" Visible="false"/>
						<asp:BoundField DataField="cco_numero" HeaderText="CCO_NUMERO" Visible="false"/>
						<asp:TemplateField>
							<ItemTemplate>                          
							<asp:ImageButton ID="AddButton" Width="30%" runat="server" ImageUrl="imagenes/der_up.png"
								CommandName="Seleccionar" CommandArgument="<%# CType(Container,GridViewRow).RowIndex %>" />                        
							</ItemTemplate> 
						</asp:TemplateField>
						</Columns>
					</asp:GridView>
				</div>
				<div class="col-md-1">
				</div>
			</asp:Panel>
		</div>
			<div class="row" >
			<asp:Panel ID="pnlContrato" runat="server" class="panel-body" Visible="false">
				<div class="col-md-2">
				</div>
				<div class="col-md-8">
					  <h4>ARTÍCULOS ESPECÍFICOS</h4>
					<asp:GridView ID="grvContrato" Width="80%" class="table tbl-sel" runat="server" AutoGenerateColumns="false">   
						<HeaderStyle HorizontalAlign="Center" />                 
					   <RowStyle HorizontalAlign="center"/>                              
						<Columns>
						<asp:BoundField DataField="codigo" HeaderText="Codigo"  />
						<asp:BoundField DataField="concepto" HeaderText="Concepto"/>  
						<asp:BoundField DataField="adicional" HeaderText="Descripción Adicional"/>                        

						<asp:TemplateField>
							<ItemTemplate>                          
							<asp:ImageButton ID="AddButton" Width="30%" runat="server" ImageUrl="imagenes/der_up.png"
								CommandName="Seleccionar" CommandArgument="<%# CType(Container,GridViewRow).RowIndex %>" />                        
							</ItemTemplate> 
						</asp:TemplateField>
						</Columns>
					</asp:GridView>
				</div>
				<div class="col-md-2">
				</div>
			</asp:Panel>
		</div>
		<div class="row">
			<asp:Panel ID="pnlRequi" runat="server" class="panel-body" Visible="false">
			<div class="col-md-12">
				  <h4>DETALLE DE LA REQUISICIÓN</h4>
				<asp:GridView ID="grvDetRequi" class="table" runat="server" AutoGenerateColumns="false">   
					<HeaderStyle HorizontalAlign="Center" />                 
					<RowStyle HorizontalAlign="center"/>                              
					<Columns>    
						<asp:BoundField DataField="indice" HeaderText="INDICE"/>   
						<asp:BoundField DataField="codigo" HeaderText="CODIGO"/>   
						<asp:BoundField DataField="concepto" HeaderText="CONCEPTO"/>                                                         
						<asp:BoundField DataField="cantidad" HeaderText="CANTIDAD"/>
						<asp:BoundField DataField="precio" HeaderText="PRECIO" Visible="false"/>
						<asp:BoundField DataField="cco_id" HeaderText="CCO_ID" Visible="false"/>
						<asp:BoundField DataField="cco_numero" HeaderText="CCO_NUMERO" Visible="false"/>
						<asp:BoundField DataField="contrato" HeaderText="CONTRATO" Visible="false"/>
						<asp:BoundField DataField="proveedor" HeaderText="PROVEEDOR" Visible="false"/>
						<asp:TemplateField>
							<ItemTemplate>                          
							<asp:ImageButton ID="AddButton" runat="server" ImageUrl="imagenes/dismiss.png"
								CommandName="Eliminar" CommandArgument="<%# CType(Container,GridViewRow).RowIndex %>" />                        
							</ItemTemplate> 
						</asp:TemplateField>
					
					</Columns>
				</asp:GridView> 
			</div>
		   </asp:Panel>
		</div>  
                                          

	<div class="row panel-body" style="color:black">
		<div class="form-group col-lg-10 col-md-10 col-sm-10">
														 
		</div>
	 
		<div class="form-group col-lg-2 col-md-2 col-sm-2" style="color:rgba(250, 184, 28, 0.50)">
			<asp:Button ID="btnGuardar" runat="server"  Text="Guardar Requisición" class="btn btn-default" />                
		</div> 
	</div>
  
<%-- <script>
	 //document.getElementById('ContentPlaceHolder1_btnAgregars').class = 'btn btn-default';
	</script>--%>

<%--      <div>
		<iframe id="ifrRequisicion" src="" width="100%" height="500" style="border-width:0" runat="server"></iframe>
	</div> 
	--%>

</asp:Content>

