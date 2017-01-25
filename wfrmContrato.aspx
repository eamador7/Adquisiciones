<%@ Page Language="VB" MasterPageFile="MPCompras.master"  AutoEventWireup="false" CodeFile="wfrmContrato.aspx.vb" Inherits="wfrmContrato" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link rel="stylesheet" href="http://localhost:1741/code.jquery.com/ui/1.11.4/themes/smoothness/jquery-ui.css"/>
    <script src="//code.jquery.com/jquery-1.10.2.js"></script>
    <script src="//code.jquery.com/ui/1.11.4/jquery-ui.js"></script>

    <%-- Reemplazar este codigo con la funcion "Buscar Proveedor", cuya funcionalidad es parecida a "Buscar Empleado"
     con la diferencia, como su nombre lo indica, que esta funcion llamará a la forma utilizada para buscar proveedor (la cual
     no se ha creado aun)--%>
<%--    <script>
        function fnBuscaEmpleado()
        {
            window.location.assign('/almacen/wfrmBuscaEmpleado.aspx?key=<%=Request.QueryString("key").Replace("+", "%2B")%>')
        }
	</script>--%>
    <style>
        .tbl-txt {
            font-size:.7em;
        }
	</style>
</asp:Content>
    
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">   

    <asp:Timer ID="Timer1" runat="server" Enabled="false" Interval="3000"></asp:Timer>
    <div class="panel panel-success">
        <div class="panel-body">
            <div class="row">
                <div class="form-group col-lg-1 col-md-1 col-sm-1"> 
                    <asp:label ID="lblFolio" runat="server" ForeColor="Red">FOLIO:</asp:label>               
                    <asp:TextBox ID="txtFolio"  ReadOnly="true" class="form-control" runat="server" ToolTip="Folio"></asp:TextBox>                
                </div> 
              
                <div class="form-group col-lg-5 col-md-5 col-sm-5">                
                    <asp:label ID="lblNombre" runat="server">NOMBRE:</asp:label>
                    <asp:TextBox ID="txtNombre" ReadOnly="false" class="form-control" runat="server" ToolTip="Nombre" placeholder="Nombre"></asp:TextBox>
                </div>  
                <div class="form-group col-lg-4 col-md-4 col-sm-4">
                    <asp:label ID="lblcontrato" runat="server">NO. CONTRATO:</asp:label>
                    <asp:TextBox ReadOnly="false" ID="txtContrato" class="form-control" runat="server" ToolTip="No. Contrato" placeholder="No. Contrato"></asp:TextBox>                
                </div>    
                  <div class="form-group col-lg-2 col-md-2 col-sm-2">                
                    <asp:label ID="lblFecha" runat="server">FECHA:</asp:label>               
                    <asp:TextBox ID="txtFecha" ReadOnly="true" class="form-control" runat="server" ToolTip="Fecha" placeholder="Fecha"></asp:TextBox>                 
                </div>                                                      
            </div>
        
            <div class="row">                    
                 <div class="form-group col-lg-9 col-md-9 col-sm-9">
                    <asp:label ID="Label1" runat="server">OBSERVACIONES:</asp:label>  
                    <asp:TextBox ID="txtObservaciones" TextMode="MultiLine" class="form-control" runat="server" ToolTip="Observaciones" placeholder="Observaciones"></asp:TextBox>                
                </div>                
            </div>
            <div class="row">   
                <div class="form-group col-lg-2 col-md-2 col-sm-2"> 
                    <asp:label ID="Label2" runat="server">NO. PROVEEDOR:</asp:label>               
                    <asp:TextBox ID="txtProveedor" ReadOnly="false" class="form-control" runat="server" ToolTip="No. Proveedor" placeholder="No. Proveedor"></asp:TextBox>                
                </div>          
                  <div class="form-group col-lg-4 col-md-4 col-sm-4"> 
                    
                    <asp:TextBox ID="txtProveedorNombre" ReadOnly="true" class="form-control" runat="server" ToolTip="Nombre Proveedor" placeholder="Nombre Proveedor"></asp:TextBox>                
                </div>        
                <%--Funcion ni ventana escritas aun--%>
                <div class="form-group col-lg-2 col-md-2 col-sm-2">
                    <asp:Button ID="btnBuscar" runat="server" Text="Buscar" class="btn btn-default" Visible="false" />
                    <input type="button" id="btnBuscarProveedor" value="Buscar Proveedor" onclick="fnBuscaProveedor()" class="btn btn-default" />
                </div>  
            </div>                           
                   
            
        </div>
    </div>
    <div class="panel panel-success">
        <div class="row panel-body" style="color:black;padding-bottom: 0px;">
            <div class="form-group col-lg-2 col-md-2 col-sm-2">                
                    <asp:TextBox ID="txtCodigo" runat="server" class="form-control" ToolTip="Codigo" placeholder="Codigo"></asp:TextBox>                                           
            </div>
            <div class="form-group col-lg-5 col-md-5 col-sm-5" style="color:rgba(250, 184, 28, 0.50)">
                <asp:TextBox ID="txtConcepto" runat="server" class="form-control" ToolTip="Concepto" placeholder="Concepto"></asp:TextBox>
            </div>   
            <div class="form-group col-lg-1 col-md-1 col-sm-1"> 
                <asp:LinkButton ID="btnBuscarProducto" runat="server" Text="Buscar" class="btn btn-default" />
            </div>                      
            <div class="form-group col-lg-2 col-md-2 col-sm-2">
                <asp:TextBox ID="txtCantidadMin" runat="server" class="form-control" ToolTip="Cantidad Mínima" placeholder="Cantidad Mínima"></asp:TextBox>                                          
            </div>  
            <div class="form-group col-lg-2 col-md-2 col-sm-2">
                <asp:TextBox ID="txtCantidadMax" runat="server" class="form-control" ToolTip="Cantidad Máxima" placeholder="Cantidad Máxima"></asp:TextBox>                                          
            </div>                                                            
        </div>
        <div class="row panel-body" style="color:black;padding-top: 0px;">
            <div class="form-group col-lg-9 col-md-9 col-sm-9">
            </div>
                   
          
            <div class="form-group col-lg-1 col-md-1 col-sm-1"> 
                <asp:LinkButton ID="btnLimpiar" runat="server" Text="Limpiar" class="btn btn-default" />
            </div>
            <div class="form-group col-lg-2 col-md-2 col-sm-2"> 
                <asp:LinkButton  ID="btnAgregarProducto" runat="server" Text="Agregar Articulo" class="btn btn-default"  />
            </div>
        </div>
        <div class="row" align="center">
			<asp:Panel ID="pnlArticulos" runat="server" class="panel-body" Visible="false">
                <div class="col-md-1">
                </div>
                <div class="col-md-10">
                    <asp:GridView ID="grvArticulos" Width="80%" class="table tbl-sel" runat="server" AutoGenerateColumns="false">   
                        <HeaderStyle HorizontalAlign="Center" />                 
                       <RowStyle HorizontalAlign="center"/>                              
                        <Columns>
                        <asp:BoundField DataField="codigo" HeaderText="Codigo"/>
                        <asp:BoundField DataField="concepto" HeaderText="Concepto"/>                        
                        <asp:BoundField DataField="udm" HeaderText="UDM"/>       
                       
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
        <div class="row">
            <div class="col-md-12">
                <asp:GridView ID="grvPartidas" class="table" runat="server" AutoGenerateColumns="false">   
                    <HeaderStyle HorizontalAlign="Center" />                 
                    <RowStyle HorizontalAlign="center"/>                              
                    <Columns>    
                        <asp:BoundField DataField="partida" HeaderText="PARTIDA"/>   
                        <asp:BoundField DataField="codigo" HeaderText="CODIGO"/>   
                        <asp:BoundField DataField="concepto" HeaderText="CONCEPTO"/>                                                         
                        <asp:BoundField DataField="cantidadmin" HeaderText="CANTIDADMIN"/>
                        <asp:BoundField DataField="cantidadmax" HeaderText="CANTIDADMAX"/>
                        <asp:BoundField DataField="udm" HeaderText="UDM"/>
    
                        <asp:TemplateField>
                            <ItemTemplate>                          
                            <asp:ImageButton ID="AddButton" runat="server" ImageUrl="imagenes/dismiss.png"
                                CommandName="Eliminar" CommandArgument="<%# CType(Container,GridViewRow).RowIndex %>" />                        
                            </ItemTemplate> 
                        </asp:TemplateField>
                    
                    </Columns>
                </asp:GridView> 
            </div>
        </div>                                         
    </div>
    <div class="row panel-body" style="color:black">
        <div class="form-group col-lg-10 col-md-10 col-sm-10">
                                                         
        </div>
        <div class="form-group col-lg-2 col-md-2 col-sm-2" style="color:rgba(250, 184, 28, 0.50)">
            <asp:Button ID="btnGuardar" runat="server"  Text="Guardar Contrato" class="btn btn-default" />                
        </div> 
    </div>
 <script>
     document.getElementById('ContentPlaceHolder1_btnAgregars').class = 'btn btn-default';
	</script>
</asp:content>
