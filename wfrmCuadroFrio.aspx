<%@ Page Language="VB" MasterPageFile="MPCompras.master" AutoEventWireup="false" CodeFile="wfrmCuadroFrio.aspx.vb" Inherits="wfrmCuadroFrio" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

</asp:Content>
    
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">  
    <asp:HiddenField ID="hdnIdRequisicionSel" runat="server" />
    <asp:HiddenField ID="hdnRequisicionSelIndex" runat="server" />
    <asp:HiddenField ID="hdnOrdenSelIndex" runat="server" />
    <asp:HiddenField ID="hdnOrden" runat="server" />
    <asp:HiddenField ID="hdnProveedor" runat="server" />
    <asp:HiddenField ID="hdnCodigo" runat="server" />
    <asp:HiddenField ID="hdnCcoId" runat="server" />
    <asp:HiddenField ID="hdnPrecio" runat="server" />
    <asp:HiddenField ID="hdnRequisicion" runat="server" />
    <asp:HiddenField ID="hdnDetallerequisicion" runat="server" />
    <asp:HiddenField ID="hdnDetalleIndex" runat="server" />
    <asp:HiddenField ID="hdnCotizacionIndex" runat="server" />
        <asp:Timer ID="Timer1" runat="server" Enabled="false" Interval="3000"></asp:Timer>
        <div class="panel panel-success">
            

             
      <asp:Panel class="panel-body"  ID="pnlRequis" runat="server" Visible="True">     
            <div class="row">
                <div class="col-md-12">
                     <h4>REQUISICIONES:</h4>
                    <asp:GridView ID="grvRequisiciones" class="table tbl-sel" runat="server" AutoGenerateColumns="false" ShowHeaderWhenEmpty="True">   
                        <HeaderStyle HorizontalAlign="Center" />                 
                        <RowStyle HorizontalAlign="center"/>                              
                        <Columns>                                                                
                            <asp:BoundField DataField="numrequisicion" HeaderText="# REQUISICION"/>                        
                            <asp:BoundField DataField="solicitante" HeaderText="SOLICITADO POR"/>
                            <asp:BoundField DataField="departamento" HeaderText="DEPARTAMENTO"/> 
                            <asp:BoundField DataField="fecha" HeaderText="FECHA"/>   
                            <asp:BoundField DataField="estatus" HeaderText="ESTATUS"/> 
                            <asp:BoundField DataField="nombreContrato" HeaderText="CONTRATO"/> 
                            <asp:TemplateField>
                                <ItemTemplate>                          
                                <asp:ImageButton ID="SelButton" runat="server" Height="15" ImageUrl="imagenes/der_up.png"
                                CommandName="Seleccionar" CommandArgument="<%# CType(Container,GridViewRow).RowIndex %>" />                        
                                </ItemTemplate> 
                            </asp:TemplateField>
                    
                        </Columns>
                    </asp:GridView> 
                </div>
            </div> 
      </asp:Panel>
      <asp:Panel class="panel-body"  ID="pnlSelected" runat="server" Visible="false">     
            <div class="row">
                <div class="col-md-12">
                     <h4>REQUISICION DE COMPRA:</h4>
                    <asp:GridView ID="grvSelected" class="table tbl-sel" runat="server" AutoGenerateColumns="false" ShowHeaderWhenEmpty="True">   
                        <HeaderStyle HorizontalAlign="Center" />                 
                        <RowStyle HorizontalAlign="center"/>                              
                        <Columns>                                                                
                            <asp:BoundField DataField="numrequisicion" HeaderText="# REQUISICION"/>                        
                            <asp:BoundField DataField="solicitante" HeaderText="SOLICITADO POR"/>
                            <asp:BoundField DataField="departamento" HeaderText="DEPARTAMENTO"/> 
                            <asp:BoundField DataField="fecha" HeaderText="FECHA"/>   
                            <asp:BoundField DataField="total" HeaderText="TOTAL"/> 

                    
                        </Columns>
                    </asp:GridView> 
                </div>
            </div> 
      </asp:Panel>
      <asp:Panel class="panel-body"  ID="pnlDetalle" runat="server" Visible="false">     
             <div class="row">
                <div class="col-md-12">
                    <h4>DETALLE REQUISICION:</h4>
                    <asp:GridView ID="grvDetalle" class="table tbl-sel" runat="server" AutoGenerateColumns="false" ShowHeaderWhenEmpty="True">   
                        <HeaderStyle HorizontalAlign="Center" />                 
                        <RowStyle HorizontalAlign="center"/>                              
                        <Columns>                                                                
                            <asp:BoundField DataField="numrequisicion" HeaderText="# REQUISICION"/>    
                            <asp:BoundField DataField="indice" HeaderText="INDICE"/>
                            <asp:BoundField DataField="codigo" HeaderText="CODIGO"/>                            
                            <asp:BoundField DataField="concepto" HeaderText="CONCEPTO"/>    
                            <asp:BoundField DataField="cantidad" HeaderText="CANTIDAD"/>    
                       
                            
                            <asp:TemplateField>
                                <ItemTemplate>                          
                                <asp:ImageButton ID="SelDetButton" runat="server" Height="15" ImageUrl="imagenes/der_up.png"
                                CommandName="Seleccionar" CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>" />                        
                                </ItemTemplate> 
                            </asp:TemplateField>
                    
                        </Columns>
                    </asp:GridView> 

                </div>
            </div>
          </asp:Panel>

           <hr />
            <asp:Panel class ="panel panel-body"  ID="cotizacionesPanel" Visible="False" runat="server">
            <div class="row">
                <div class="col-md-12">
                      <h4>COTIZACIONES:</h4>
                   
                            <asp:GridView ID="grvCotizaciones" class="table" runat="server" AutoGenerateColumns="false">   
                                <HeaderStyle HorizontalAlign="Center" />                 
                                <RowStyle HorizontalAlign="center"/>                              
                                <Columns>    
                                    <asp:BoundField DataField="indice" HeaderText="INDICE" Visible="true"/>   
                                    <asp:BoundField DataField="codigo" HeaderText="CODIGO" Visible="False"/>   
                                    <asp:BoundField DataField="concepto" HeaderText="CONCEPTO" Visible="False"/>                                                         
                                    <asp:BoundField DataField="numeroproveedor" ReadOnly="True" HeaderText="NO. PROVEEDOR"/>
                                    <asp:BoundField DataField="nombreproveedor" ReadOnly="True" HeaderText="NOMBRE" Visible="true"/>
                                    <asp:BoundField DataField="precio" HeaderText="PRECIO" Visible="true"/>
                                    <asp:BoundField DataField="marca" ReadOnly="True" HeaderText="MARCA" Visible="true"/>
                                    <asp:BoundField DataField="tiempoEntrega" ReadOnly="True" HeaderText="TIEMPO DE ENTREGA" Visible="true"/>
                                    <asp:BoundField DataField="requisicion" HeaderText="CCO_NUMERO" Visible="false"/>
                                    <asp:BoundField DataField="detallerequisicion" HeaderText="CONTRATO" Visible="false"/>
                                    <asp:TemplateField Visible="false">
                                        <ItemTemplate>
                                            <asp:LinkButton Text="Editar" runat="server" CommandName="Edit" />
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:LinkButton Text="Actualizar" runat="server" OnClick="OnUpdate" />
                                            <asp:LinkButton Text="Cancelar" runat="server" OnClick="OnCancel" />
                                        </EditItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <ItemTemplate>                          
                                        <asp:ImageButton ID="SelCotiButton" runat="server" Height="15" ImageUrl="imagenes/der_up.png"
                                            CommandName="Seleccionar" CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>" />    
                                        <asp:ImageButton ID="EditarCotizacionButton" Height="15" runat="server" ImageUrl="imagenes/edit.png"
                                            CommandName="Editar" CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>" />                       
                                        </ItemTemplate> 
                                    </asp:TemplateField>
                    
                                </Columns>
                            </asp:GridView> 
 
                </div>
        </div>  
        </asp:Panel>
          <hr />    
         <div class="row panel-body" style="color:black">
            <div class="form-group col-lg-6 col-md-6 col-sm-6">
            
            </div>
                <div class="form-group col-lg-2 col-md-2 col-sm-2">
                <asp:CheckBox runat="server" id="cbComite" Visible="False" Text="Pasar a Comité"/>        
                </div>  
             <div class="form-group col-lg-2 col-md-2 col-sm-2" style="color:rgba(250, 184, 28, 0.50)">
                <asp:Button ID="btnCancelar" runat="server" Visible="false"  Text="Cancelar " class="btn btn-default" />                
            </div> 
            <div class="form-group col-lg-2 col-md-2 col-sm-2" style="color:rgba(250, 184, 28, 0.50)">
                <asp:Button ID="btnGuardar" runat="server" Visible="False"  Text="Guardar" class="btn btn-default" />                
            </div> 
        </div>
        <div>
        </div>
            
        </div> 
    
</asp:Content>
