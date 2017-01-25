<%@ Page Language="VB" MasterPageFile="MPCompras.master" AutoEventWireup="false" CodeFile="wfrmBuscaProveedor.aspx.vb" Inherits="wfrmBuscaProveedor" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link rel="stylesheet" href="//code.jquery.com/ui/1.11.4/themes/smoothness/jquery-ui.css"/>
    <script src="//code.jquery.com/jquery-1.10.2.js"></script>
    <script src="//code.jquery.com/ui/1.11.4/jquery-ui.js"></script>
    <script>
        $(function () {
            $("#dialog").dialog(
                {
                    //autoOpen: false,
                    appendTo: "form",
                    modal: true,
                    width: 800,
                    closeOnEscape: false
                });
        });
	</script>
    <style>
        .tbl-txt {
            font-size:.7em;
        }
        .ui-dialog-titlebar-close {
          visibility: hidden;
        }
	</style>
</asp:Content>
    
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">  
    
            <div id="dialog" title="Buscar Proveedor" style="width: 60%; ">
		        <div class="panel panel-success">
                    <div class="panel-body">
                        <div class="row">    
                            <div class="form-group col-lg-2 col-md-2 col-sm-2"> 
                                <asp:label ID="Label1" CssClass="tbl-txt" runat="server">NO. PROVEEDOR:</asp:label>               
                                <asp:TextBox ID="txtNumProveedor" class="form-control tbl-txt" runat="server" ToolTip="No. Proveedor" placeholder="No. Proveedor"></asp:TextBox>                
                            </div>
                            <div class="form-group col-lg-8 col-md-8 col-sm-8"> 
                                <asp:label ID="Label5" CssClass="tbl-txt" runat="server">PROVEEDOR:</asp:label>               
                                <asp:TextBox ID="txtNombreBuscar" class="tbl-txt form-control" runat="server" ToolTip="Nombre" placeholder="Nombre"></asp:TextBox>                
                            </div>                                        
                            <div class="form-group col-lg-2 col-md-2 col-sm-2">
                                <asp:linkButton ID="btnBuscarProveedor" runat="server" Text="Buscar" class="btn btn-default" />
                            </div>                                                                                                      
                        </div>
                        <div class="row">                    
                            <div class="col-md-12">
                                <asp:GridView ID="grvProveedor" class="table tbl-sel tbl-txt" runat="server" AutoGenerateColumns="false">   
                                    <HeaderStyle HorizontalAlign="Center" />                 
                                    <RowStyle HorizontalAlign="center"/>                            
                                    <Columns>                                                                
                                        <asp:BoundField DataField="noProveedor" HeaderText="NO. PROVEEDOR"/>                        
                                        <asp:BoundField DataField="nombre" HeaderText="NOMBRE"/>
                                        <asp:BoundField DataField="domicilio" HeaderText="DOMICILIO" Visible="false"/>
                                        <asp:BoundField DataField="ciudad" HeaderText="CIUDAD" Visible="false"/> 
                                        <asp:BoundField DataField="rfc" HeaderText="RFC" Visible="false"/>
                                        <asp:BoundField DataField="nomfis" HeaderText="NOMBRE FISCAL"/>
                                         <asp:TemplateField>
                                            <ItemTemplate>                          
                                            <asp:ImageButton ID="AddButton" Width="40%" runat="server" ImageUrl="imagenes/der_up.png"
                                                CommandName="Seleccionar" CommandArgument="<%# CType(Container,GridViewRow).RowIndex %>" />                        
                                            </ItemTemplate> 
                                        </asp:TemplateField>
                    
                                    </Columns>
                                </asp:GridView> 
                            </div> 
                        </div> 
                    </div>  
                </div> 				    
	        </div>
        
</asp:content>
