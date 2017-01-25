<%@ Page Language="VB" MasterPageFile="MPCompras.master" AutoEventWireup="false" CodeFile="wfrmSolicitarCotizacion.aspx.vb" Inherits="wfrmSolicitarCotizacion" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

</asp:Content>
    
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">  
    <%--<asp:HiddenField ID="hdnIdRequisicionSel" runat="server" />
    <asp:HiddenField ID="hdnProfileId" runat="server" />--%>
    <asp:HiddenField ID="hdnRequisicionSelIndex" runat="server" />
    <asp:HiddenField ID="hdnOrdenSelIndex" runat="server" />
    <asp:HiddenField ID="hdnOrden" runat="server" />
    <asp:HiddenField ID="hdnProveedor" runat="server" />
    <asp:HiddenField ID="hdnNumeroProveedor" runat="server" />
    <asp:HiddenField ID="hdnNombreProveedor" runat="server" />
    <asp:HiddenField ID="hdnCodigo" runat="server" />
    <asp:HiddenField ID="hdnCcoId" runat="server" />
    <asp:HiddenField ID="hdnCcoNumero" runat="server" />
    <asp:HiddenField ID="hdnRequisicion" runat="server" />
    <asp:HiddenField ID="hdnDetallerequisicion" runat="server" />
    <asp:HiddenField ID="hdnConcepto" runat="server" />  
    <asp:HiddenField ID="hdnCantidad" runat="server" />  
    <asp:HiddenField ID="hdnProfileId" runat="server" />
        <asp:Timer ID="Timer1" runat="server" Enabled="false" Interval="3000"></asp:Timer>
        <div class="panel panel-info">
          <div class="panel-heading">Instrucciones</div>
          <div class="panel-body">Paso 1: Seleccione los artículos a cotizar. <br/>
              Paso 2: Seleccione los proveedores a los cuales se les enviará la solicitud. <br/>
              Paso 3: Generar la solicitud.
          </div>
        </div>
        <div class="panel panel-success">
            

             <hr />
        <asp:Panel ID="mainArtPanel" runat="server" class="panel-body" Visible="true"> 
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
                                 <asp:ImageButton ID="AddButton" runat="server" Height="15" ImageUrl="imagenes/ok.png"
                                CommandName="Aprobar" CommandArgument="<%# CType(Container,GridViewRow).RowIndex %>" />                 
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
                  <h4>ARTÍCULOS A COTIZAR:</h4>
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
         </asp:panel>   
     
            
        <hr />
              <asp:Panel ID="mainProvPanel" runat="server" class="panel-body" Visible="false"> 
                <h4>BÚSQUEDA DE PROVEEDORES:</h4>
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

                            <div class="form-group col-lg-1 col-md-1 col-sm-1">
                                &nbsp  
                                <asp:linkButton ID="btnLimpiar" runat="server" Text="Limpiar Lista" class="btn btn-default" />
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
                                 <asp:ImageButton ID="AddProvButton" runat="server" Height="15" ImageUrl="imagenes/ok.png"
                                CommandName="AgregarProveedor" CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>" />                 
                                </ItemTemplate> 
                                        </asp:TemplateField>
                    
                                    </Columns>
                                </asp:GridView> 
                            </div> 
                              </asp:Panel>
                </div> 
            <div class="row panel-body">
                 <h4>PROVEEDORES SELECCIONADOS:</h4>
            <div class="col-md-12">
                  
                <asp:GridView ID="grvProveedores" class="table" runat="server" AutoGenerateColumns="false">   
                    <HeaderStyle HorizontalAlign="Center" />                 
                    <RowStyle HorizontalAlign="center"/>                              
                    <Columns>    
                            <asp:BoundField DataField="noProveedor" HeaderText="ID"/>    
                            <asp:BoundField DataField="nombre" HeaderText="NOMBRE"/>    
                   <%--         <asp:BoundField DataField="mail" HeaderText="E-MAIL"/>    
                            <asp:BoundField DataField="telefono" HeaderText="TELEFONO"/>--%>
                            
                          <asp:TemplateField>
                            <ItemTemplate>                          
                            <asp:ImageButton ID="DelProButton" runat="server" Height="15"  ImageUrl="imagenes/delete.png"
                                CommandName="Eliminar" CommandArgument="<%# CType(Container, GridViewRow).RowIndex%>" />                        
                            </ItemTemplate> 
                        </asp:TemplateField>
                    
                    </Columns>
                </asp:GridView> 
            </div>
        </div>  
        </asp:panel>

          <hr />
         <div class="row panel-body" style="color:black">
            <div class="form-group col-lg-2 col-md-2 col-sm-2" >
                 <asp:Label id="diasLabel" runat="server">Tiempo de entrega</asp:Label>
                <asp:TextBox ID="txtDias" runat="server"  placeholder="Dias" Textmode="Number" class="btn btn-default" />                
            </div> 
            <div class="form-group col-lg-4 col-md-4 col-sm-4">
              
            </div>
         
             <div class="form-group col-lg-2 col-md-2 col-sm-2">
                 <asp:Button ID="btnSiguiente" runat="server"  Text="Siguiente" class="btn btn-default" />                
             </div>  
             <div class="form-group col-lg-2 col-md-2 col-sm-2" style="color:rgba(250, 184, 28, 0.50)">
                <asp:Button ID="btnCancelar" runat="server"  Text="Cancelar" class="btn btn-default" />                
            </div> 
            <div class="form-group col-lg-2 col-md-2 col-sm-2" style="color:rgba(250, 184, 28, 0.50)">
                <asp:Button ID="btnGuardar" runat="server"  Text="Generar Solicitud" Visible="false" class="btn btn-default" />                
            </div> 
        </div>


          <iframe id="ifrSolicitud" src="" width="100%" height="500" style="border-width:0" runat="server"></iframe>  
        </div> 
    
</asp:Content>
