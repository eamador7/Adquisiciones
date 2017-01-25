<%@ Page Language="VB" MasterPageFile="MPCompras.master" AutoEventWireup="false" CodeFile="wfrmComiteAdquisiciones.aspx.vb" Inherits="wfrmComiteAdquisiciones" %>
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
    <asp:HiddenField ID="hdnPrecio" runat="server" />
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
                                    <asp:BoundField DataField="marca" HeaderText="MARCA" Visible="true"/>
                                    <asp:BoundField DataField="tiempoEntrega" HeaderText="TIEMPO DE ENTREGA" Visible="true"/>
                                    <asp:TemplateField>
                                        <ItemTemplate>                          
                                        <asp:ImageButton ID="AddButton" runat="server" ImageUrl="imagenes/der_up.png"
                                            CommandName="Seleccionar" CommandArgument="<%# CType(Container,GridViewRow).RowIndex %>" />                        
                                        </ItemTemplate> 
                                    </asp:TemplateField>
                    
                                </Columns>
                            </asp:GridView> 
 
                </div>
            </div>  
        </div>


         
        </div> 
    
</asp:Content>
