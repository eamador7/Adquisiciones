<%@ Page Language="VB" MasterPageFile="MPCompras.master" AutoEventWireup="false" CodeFile="wfrmAutorizarPresupuesto.aspx.vb" Inherits="wfrmAutorizarPresupuesto" %>
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
    <asp:HiddenField ID="hdnCcoNumero" runat="server" />
    <asp:HiddenField ID="hdnRequisicion" runat="server" />
    <asp:HiddenField ID="hdnDetallerequisicion" runat="server" />
    <asp:HiddenField ID="hdnConcepto" runat="server" />  
    <asp:HiddenField ID="hdnCantidad" runat="server" />  
    <asp:HiddenField ID="hdnProfileId" runat="server" />
    <asp:HiddenField ID="hdnRequiIndex" runat="server" />
        <asp:Timer ID="Timer1" runat="server" Enabled="false" Interval="3000"></asp:Timer>
        <div class="panel panel-success">
            


             <hr />

      <div class="panel-body">     
            <div class="row">
                <div class="col-md-12">
                     <h4>REQUISICIONES:</h4>
                    <asp:GridView ID="grvSolicitudes" class="table tbl-sel" runat="server" AutoGenerateColumns="false" ShowHeaderWhenEmpty="True">   
                        <HeaderStyle HorizontalAlign="Center" />                 
                        <RowStyle HorizontalAlign="center"/>                              
                        <Columns>                                                                
                            <asp:BoundField DataField="numrequisicion" HeaderText="# REQUISICION"/>                        
                            <asp:BoundField DataField="solicitante" HeaderText="SOLICITADO POR"/>
                            <asp:BoundField DataField="departamento" HeaderText="DEPARTAMENTO"/> 
                            <asp:BoundField DataField="fecha" HeaderText="FECHA"/>   
                            <asp:BoundField DataField="estatus" HeaderText="ESTATUS"/> 
                            <asp:BoundField DataField="nombreContrato" HeaderText="CONTRATO"/> 
                            <asp:BoundField DataField="total" HeaderText="TOTAL"/> 
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
        <hr/>
        <asp:Panel runat="server" id="pnlInfo" Visible="false">
          <div class="row">
              <div class="col-md-3">
                   <asp:Label CssClass="h4" runat="server" >Departamento:</asp:Label>
                   <ASP:Label CssClass="h5" runat="server" ID="lblDepartamento" > </ASP:Label>

                </div>
                <div class="col-md-3">
                   <asp:Label CssClass="h4" runat="server" >Total Presupuesto:</asp:Label>
                   <ASP:Label CssClass="h5" runat="server" ID="lblTotal" > </ASP:Label>

                </div>
              <div class="col-md-3">
                   <asp:Label CssClass="h4" runat="server" >Materiales:</asp:Label>
                   <ASP:Label CssClass="h5" runat="server" ID="lblMateriales" > </ASP:Label>

                </div>
              <div class="col-md-3">
                   <asp:Label CssClass="h4" runat="server" >Servicios:</asp:Label>
                   <ASP:Label CssClass="h5" runat="server" ID="lblServicios" > </ASP:Label>

                </div>
            </div>
         </asp:Panel>
          <hr/>
      <div class="row">
                <div class="col-md-12">
                    <h4>DETALLE REQUISICION:</h4>
                    <asp:GridView ID="grvRequisiciones" class="table tbl-sel" runat="server" AutoGenerateColumns="false" ShowHeaderWhenEmpty="True">   
                        <HeaderStyle HorizontalAlign="Center" />                 
                        <RowStyle HorizontalAlign="center"/>                              
                        <Columns>                                                                
                            <asp:BoundField DataField="numrequisicion" HeaderText="# REQUISICION"/>    
                            <asp:BoundField DataField="indice" HeaderText="INDICE"/>
                            <asp:BoundField DataField="codigo" HeaderText="CODIGO"/>                            
                            <asp:BoundField DataField="concepto" HeaderText="CONCEPTO"/>    
                            <asp:BoundField DataField="cantidad" HeaderText="CANTIDAD"/> 
                            <asp:BoundField DataField="precio" HeaderText="PRECIO"/>    
                            <asp:BoundField DataField="solicitante" HeaderText="SOLICITADO POR"/>
                            <asp:BoundField DataField="departamento" HeaderText="DEPARTAMENTO"/> 
                            <asp:BoundField DataField="fecha" HeaderText="FECHA"/>   
                            <asp:BoundField DataField="estatus" HeaderText="ESTATUS"/> 
                            <asp:TemplateField>
                                <ItemTemplate>                          
                                <asp:ImageButton ID="AddMaterial" runat="server" Height="15" ImageUrl="imagenes/M.png"
                                CommandName="Material" CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>" />  
                                <asp:ImageButton ID="AddService" runat="server" Height="15" ImageUrl="imagenes/S.png"
                                CommandName="Service" CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>" />  
                                <asp:ImageButton ID="CancelButton" runat="server" Height="15" ImageUrl="imagenes/cancel.png"
                                CommandName="Cancelar" CommandArgument="<%# CType(Container,GridViewRow).RowIndex %>" />                         
                                </ItemTemplate> 
<%--                                <ItemTemplate>                          
                                <asp:ImageButton ID="CancelButton" runat="server" Height="15" ImageUrl="imagenes/cancel.png"
                                CommandName="Cancelar" CommandArgument="<%# CType(Container,GridViewRow).RowIndex %>" />                        
                                </ItemTemplate> --%>
                            </asp:TemplateField>
                    
                        </Columns>
                    </asp:GridView> 

                </div>
            </div>
          </div>
            <hr />
            <asp:Panel runat="server" id="pnlMateriales" class ="panel panel-body" Visible="False">
            <div class="row">
            <div class="col-md-12">
                  <h4>Materiales aprobados:</h4>
                <asp:GridView ID="grvAprobadas" class="table" runat="server" AutoGenerateColumns="false">   
                    <HeaderStyle HorizontalAlign="Center" />                 
                    <RowStyle HorizontalAlign="center"/>                              
                    <Columns>    
                            <asp:BoundField DataField="numrequisicion" HeaderText="# REQUISICION"/>    
                            <asp:BoundField DataField="indice" HeaderText="INDICE"/>
                            <asp:BoundField DataField="codigo" HeaderText="CODIGO"/>                            
                            <asp:BoundField DataField="concepto" HeaderText="CONCEPTO"/>    
                            <asp:BoundField DataField="cantidad" HeaderText="CANTIDAD"/> 
                            <asp:BoundField DataField="precio" HeaderText="PRECIO"/>    
                            <asp:BoundField DataField="solicitante" HeaderText="SOLICITADO POR"/>
                            <asp:BoundField DataField="departamento" HeaderText="DEPARTAMENTO"/> 
                          <asp:TemplateField>
                            <ItemTemplate>                          
                            <asp:ImageButton ID="DelButton" runat="server" Height="15"  ImageUrl="imagenes/delete.png"
                                CommandName="Eliminar" CommandArgument="<%# CType(Container, GridViewRow).RowIndex%>" />                        
                            </ItemTemplate> 
                        </asp:TemplateField>
                    
                    </Columns>
                </asp:GridView> 
            </div>
        </div>  
        </asp:Panel>
            
         <hr />
            <asp:panel runat="server" id="pnlServicios" class ="panel panel-body" Visible="False">
            <div class="row">
            <div class="col-md-12">
                  <h4>Servicios aprobados:</h4>
                <asp:GridView ID="grvServicios" class="table" runat="server" AutoGenerateColumns="false">   
                    <HeaderStyle HorizontalAlign="Center" />                 
                    <RowStyle HorizontalAlign="center"/>                              
                    <Columns>    
                            <asp:BoundField DataField="numrequisicion" HeaderText="# REQUISICION"/>    
                            <asp:BoundField DataField="indice" HeaderText="INDICE"/>
                            <asp:BoundField DataField="codigo" HeaderText="CODIGO"/>                            
                            <asp:BoundField DataField="concepto" HeaderText="CONCEPTO"/>    
                            <asp:BoundField DataField="cantidad" HeaderText="CANTIDAD"/> 
                            <asp:BoundField DataField="precio" HeaderText="PRECIO"/>    
                            <asp:BoundField DataField="solicitante" HeaderText="SOLICITADO POR"/>
                            <asp:BoundField DataField="departamento" HeaderText="DEPARTAMENTO"/> 
                          <asp:TemplateField>
                            <ItemTemplate>                          
                            <asp:ImageButton ID="DelButton" runat="server" Height="15"  ImageUrl="imagenes/delete.png"
                                CommandName="Eliminar" CommandArgument="<%# CType(Container, GridViewRow).RowIndex%>" />                        
                            </ItemTemplate> 
                        </asp:TemplateField>
                    
                    </Columns>
                </asp:GridView> 
            </div>
        </div>  
        </asp:panel>
           <hr />
            <div class ="panel panel-body">
            <div class="row">
            <div class="col-md-12">
                  <h4>A CANCELAR:</h4>
                <asp:GridView ID="grvCanceladas" class="table" runat="server" AutoGenerateColumns="false">   
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
                        <asp:TemplateField>
                            <ItemTemplate>                          
                            <asp:ImageButton ID="DelButton2" runat="server" Height="15"  ImageUrl="imagenes/delete.png"
                                CommandName="Eliminar" CommandArgument="<%# CType(Container, GridViewRow).RowIndex%>" />                        
                            </ItemTemplate> 
                        </asp:TemplateField>
                    
                    </Columns>
                </asp:GridView> 
            </div>
        </div>  
        </div>
          <hr />    
         <div class="row panel-body" style="color:black">

                <div class="form-group col-lg-2 col-md-2 col-sm-5">
                        
                    </div>  
             <div class="form-group col-lg-2 col-md-2 col-sm-2" style="color:rgba(250, 184, 28, 0.50)">
                <asp:Button ID="btnCancelar" runat="server"  Text="Cancelar Cambios" class="btn btn-default" />                
            </div> 
            <div class="form-group col-lg-2 col-md-2 col-sm-2" style="color:rgba(250, 184, 28, 0.50)">
                <asp:Button ID="btnGuardar" runat="server"  Text="Guardar Cambios" class="btn btn-default" />                
            </div> 
        </div>
        <div>
        </div>
            
        </div> 
    
</asp:Content>
