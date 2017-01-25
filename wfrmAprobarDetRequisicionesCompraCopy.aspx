<%@ Page Language="VB" MasterPageFile="MPCompras.master" AutoEventWireup="false" CodeFile="wfrmAprobarDetRequisicionesCompraCopy.aspx.vb" Inherits="wfrmAprobarDetRequisicionesCompraCopy" %>
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
          <div class="panel panel-body">
         
                     <h4>FILTROS DE BÚSQUEDA:</h4>
              
                <div class="row">
                     <div class="form-group col-lg-2 col-md-2 col-sm-6">
                        <asp:label ID="Label1" runat="server">REQUISICIÓN:</asp:label>
                        <asp:TextBox ID="txtRequisicion" TextMode="Number" class="form-control" runat="server" ToolTip="Requisición" placeholder="Requisición"></asp:TextBox>                
                    </div>
                   <div class="form-group col-lg-2 col-md-2 col-sm-2">                
                  
                    </div>                                      
                    <div class="form-group col-lg-2 col-md-2 col-sm-2">                
                        
                    </div>                                      
                    <div class="form-group col-lg-2 col-md-2 col-sm-5">
                        <asp:label ID="lblFechaDesde" runat="server">FECHA DESDE:</asp:label>
                        <asp:TextBox ID="txtFechaDesde" TextMode="Date" class="form-control" runat="server" ToolTip="Fecha Desde" placeholder="Fecha Desde"></asp:TextBox>                
                    </div>  
                    <div class="form-group col-lg-2 col-md-2 col-sm-5">
                        <asp:label ID="lblFechaHasta" runat="server">FECHA HASTA:</asp:label>
                        <asp:TextBox ID="txtFechaHasta" TextMode="Date" class="form-control" runat="server" ToolTip="Fecha Hasta" placeholder="Fecha Hasta"></asp:TextBox>                
                    </div>
                    <div class="form-group col-lg-2 col-md-2 col-sm-2" style="color:rgba(250, 184, 28, 0.50)">  
                        <br />                     
                        <asp:Button ID="btnConsultar" runat="server"  Text="Consultar" class="btn btn-default" />                                        
                    </div>
                </div>
            </div>
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
                                <asp:ImageButton ID="AddButton" runat="server" Height="15" ImageUrl="imagenes/ok.png"
                                CommandName="Aprobar" CommandArgument="<%# CType(Container,GridViewRow).RowIndex %>" />  
                               <%-- <asp:ImageButton ID="CancelButton" runat="server" Height="15" ImageUrl="imagenes/cancel.png"
                                CommandName="Cancelar" CommandArgument="<%# CType(Container,GridViewRow).RowIndex %>" />    --%>                     
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
            <div class ="panel panel-body">
            <div class="row">
            <div class="col-md-12">
                  <h4>A APROBAR:</h4>
                <asp:GridView ID="grvAprobadas" class="table" runat="server" AutoGenerateColumns="false">   
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
                            <asp:ImageButton ID="DelButton" runat="server" Height="15"  ImageUrl="imagenes/delete.png"
                                CommandName="Eliminar" CommandArgument="<%# CType(Container, GridViewRow).RowIndex%>" />                        
                            </ItemTemplate> 
                        </asp:TemplateField>
                    
                    </Columns>
                </asp:GridView> 
            </div>
        </div>  
        </div>
           <hr />
<%--            <div class ="panel panel-body">
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
          <hr />   --%> 
         <div class="row panel-body" style="color:black">
            <div class="form-group col-lg-6 col-md-6 col-sm-6">
                 <asp:TextBox ID="txtObservaciones" ReadOnly="false" TextMode="MultiLine" class="form-control" runat="server" ToolTip="Observaciones" placeholder="Observaciones"></asp:TextBox>                                        
            </div>
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
