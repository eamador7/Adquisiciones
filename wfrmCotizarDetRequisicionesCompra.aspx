<%@ Page Language="VB" MasterPageFile="MPCompras.master" AutoEventWireup="false" CodeFile="wfrmCotizarDetRequisicionesCompra.aspx.vb" Inherits="wfrmCotizarDetRequisicionesCompra" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

</asp:Content>
    
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">  
    <%--<asp:HiddenField ID="hdnIdRequisicionSel" runat="server" />
    <asp:HiddenField ID="hdnProfileId" runat="server" />--%>
    <asp:HiddenField ID="hdnRequisicionSelIndex" runat="server" />
    <asp:HiddenField ID="hdnNumeroProveedor" runat="server" />
    <asp:HiddenField ID="hdnNombreProveedor" runat="server" />
    <asp:HiddenField ID="hdnCodigo" runat="server" />
    <asp:HiddenField ID="hdnRequisicion" runat="server" />
    <asp:HiddenField ID="hdnDetallerequisicion" runat="server" />
    <asp:HiddenField ID="hdnConcepto" runat="server" />  
    <asp:HiddenField ID="hdnCantidad" runat="server" />  
        <asp:Timer ID="Timer1" runat="server" Enabled="false" Interval="3000"></asp:Timer>
        <div class="panel panel-success">
         

      <div class="panel-body">      
      <div class="row">
                <div class="col-md-12">
                    <h4>DETALLE REQUISICIONES:</h4>
                    <asp:GridView ID="grvRequisiciones" class="table tbl-sel" runat="server" AutoGenerateColumns="false" ShowHeaderWhenEmpty="True">   
                        <HeaderStyle HorizontalAlign="Center" />                 
                        <RowStyle HorizontalAlign="center"/>                              
                        <Columns>                                                                
                            <asp:BoundField DataField="numrequisicion" HeaderText="# REQUISICION"/>    
                            <asp:BoundField DataField="indice" HeaderText="INDICE"/>
                            <asp:BoundField DataField="codigo" HeaderText="CODIGO"/>                            
                            <asp:BoundField DataField="concepto" HeaderText="CONCEPTO"/>    
                            <asp:BoundField DataField="cantidad" HeaderText="CANTIDAD"/>    
                            <asp:BoundField DataField="solicitante" HeaderText="SOLICITADO POR"/>
                            <asp:BoundField DataField="departamento" HeaderText="DEPARTAMENTO"/> 
                            <asp:BoundField DataField="fecha" HeaderText="FECHA"/>   
                            <asp:BoundField DataField="estatus" HeaderText="ESTATUS"/> 
                            <asp:TemplateField>
                                <ItemTemplate>                          
                                <asp:ImageButton ID="AddButton" runat="server" Height="15" ImageUrl="imagenes/der_up.png"
                                CommandName="Seleccionar" CommandArgument="<%# CType(Container,GridViewRow).RowIndex %>" />                        
                                </ItemTemplate> 
                            </asp:TemplateField>
                    
                        </Columns>
                    </asp:GridView> 
                  <div class="row">    
                            <div class="form-group col-lg-2 col-md-2 col-sm-2"> 
                                <asp:label ID="Label2" CssClass="tbl-txt" runat="server">NO. PROVEEDOR:</asp:label>               
                                <asp:TextBox ID="txtNumProveedor" class="form-control tbl-txt" runat="server" ToolTip="No. Proveedor" placeholder="No. Proveedor"></asp:TextBox>                
                            </div>
                            <div class="form-group col-lg-5 col-md-5 col-sm-5"> 
                                <asp:label ID="Label5" CssClass="tbl-txt" runat="server">PROVEEDOR:</asp:label>               
                                <asp:TextBox ID="txtNombreBuscar" class="tbl-txt form-control" runat="server" ToolTip="Nombre" placeholder="Nombre"></asp:TextBox>                
                            </div>                                        
                            <div class="form-group col-lg-1 col-md-1 col-sm-1">
                                    &nbsp         &nbsp                        
                                <asp:linkButton ID="btnBuscarProveedor" runat="server" Text="Buscar" class="btn btn-default" />
                            </div>      
                             <div class="form-group col-lg-2 col-md-2 col-sm-2"> 
                                <asp:label ID="Label1" CssClass="tbl-txt" runat="server">PRECIO:</asp:label>               
                                <asp:TextBox ID="txtPrecio" class="tbl-txt form-control" TextMode="Number" runat="server" ToolTip="Precio" placeholder="Precio"></asp:TextBox>                
                            </div>  
                            <div class="form-group col-lg-1 col-md-1 col-sm-1">
                                &nbsp  
                                <asp:linkButton ID="btnAgregar" runat="server" Text="Agregar" class="btn btn-default" />
                            </div>                                                                                                   
                        </div>  
                        <div class="row">    
                           <asp:Panel ID="pnlProveedor" runat="server" class="panel-body" Visible="false">                
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
                                            <asp:ImageButton ID="AddButton" Height="15" runat="server" ImageUrl="imagenes/der_up.png"
                                                CommandName="Seleccionar" CommandArgument="<%# CType(Container,GridViewRow).RowIndex %>" />                        
                                            </ItemTemplate> 
                                        </asp:TemplateField>
                    
                                    </Columns>
                                </asp:GridView> 
                            </div> 
                              </asp:Panel>
                        </div> 
                </div>
            </div>
          </div>
            <hr />
           <div class ="panel panel-body">
                <div class="row">
                <div class="col-md-12">
                      <h4>COTIZACIONES:</h4>
                   
                            <asp:GridView ID="grvCotizaciones" class="table" runat="server" AutoGenerateColumns="false">   
                                <HeaderStyle HorizontalAlign="Center" />                 
                                <RowStyle HorizontalAlign="center"/>                              
                                <Columns>    
                                    <asp:BoundField DataField="indice" HeaderText="INDICE"/>   
                                    <asp:BoundField DataField="codigo" HeaderText="CODIGO"/>   
                                    <asp:BoundField DataField="concepto" HeaderText="CONCEPTO"/>                                                         
                                    <asp:BoundField DataField="numeroproveedor" HeaderText="NO. PROVEEDOR"/>
                                    <asp:BoundField DataField="nombreproveedor" HeaderText="NOMBRE" Visible="true"/>
                                    <asp:BoundField DataField="precio" HeaderText="PRECIO" Visible="true"/>
                                    <asp:BoundField DataField="requisicion" HeaderText="CCO_NUMERO" Visible="false"/>
                                    <asp:BoundField DataField="detallerequisicion" HeaderText="CONTRATO" Visible="false"/>
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


                        


            <div class="form-group col-lg-5 col-md-5 col-sm-5">
                 
            </div>
                
             <div class="form-group col-lg-2 col-md-2 col-sm-2" style="color:rgba(250, 184, 28, 0.50)">
                <asp:Button ID="btnCancelar" runat="server"  Text="Cancelar Registro" class="btn btn-default" />                
            </div> 
            <div class="form-group col-lg-2 col-md-2 col-sm-2" style="color:rgba(250, 184, 28, 0.50)">
                <asp:Button ID="btnGuardar" runat="server"  Text="Guardar cotizaciones" class="btn btn-default" />                
            </div>
             <div class="form-group col-lg-2 col-md-2 col-sm-2" >
                 <asp:CheckBox ID="cbFinalizar" runat ="server" Text="Pasar a comité" />
             </div> 
        </div>

         
        </div> 
    
</asp:Content>
