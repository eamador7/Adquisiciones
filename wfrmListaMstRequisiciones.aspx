﻿<%@ Page Language="VB" MasterPageFile="MPCompras.master" AutoEventWireup="false" CodeFile="wfrmListaMstRequisiciones.aspx.vb" Inherits="wfrmListaMstRequisiciones" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

</asp:Content>
    
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">  
    <asp:HiddenField ID="hdnIdRequisicionSel" runat="server" />
    <asp:HiddenField ID="hdnProfileId" runat="server" />
    <asp:Timer ID="Timer1" runat="server" Enabled="false" Interval="3000"></asp:Timer>
        <div class="panel panel-success">
            <div class="panel-body">
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
            <div class="row">
                <div class="col-md-12">
                    <asp:GridView ID="grvSolicitudes" class="table tbl-sel" runat="server" AutoGenerateColumns="false" ShowHeaderWhenEmpty="True">   
                        <HeaderStyle HorizontalAlign="Center" />                 
                        <RowStyle HorizontalAlign="center"/>                              
                        <Columns>                                                                
                            <asp:BoundField DataField="numrequisicion" HeaderText="# REQUISICION"/>                        
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
            <div class="row">
                <div class="col-md-1">
                </div>
                <div class="col-md-10">
                    <asp:GridView ID="grvProducto" class="table" runat="server" AutoGenerateColumns="false" ShowHeaderWhenEmpty="True">   
                        <HeaderStyle HorizontalAlign="Center" />                 
                        <RowStyle HorizontalAlign="center"/>                              
                        <Columns>
                                <asp:BoundField DataField="numrequisicion" HeaderText="# REQUISICION" Visible="false"/>
                                <asp:BoundField DataField="indice" HeaderText="INDICE"/> 
                                <asp:BoundField DataField="codigo" HeaderText="CODIGO"/>                                
                                <asp:BoundField DataField="concepto" HeaderText="CONCEPTO"/>
                                <asp:BoundField DataField="cantidad" HeaderText="CANTIDAD"/> 
                                                                                                         
                        </Columns>
                    </asp:GridView> 
                </div>
                <div class="col-md-1">
                </div>
            </div>
        </div>
         <div class="row panel-body" style="color:black">
            <div class="form-group col-lg-6 col-md-6 col-sm-6">
                 <asp:TextBox ID="txtMotivoo" ReadOnly="false" TextMode="MultiLine" class="form-control" runat="server" ToolTip="Motivo Cancelacion o Modificacion" placeholder="Motivo Cancelacion o Modificacion"></asp:TextBox>                                        
            </div>
             <div class="form-group col-lg-2 col-md-2 col-sm-2" style="color:rgba(250, 184, 28, 0.50)">
                <asp:Button ID="btnCancelar" runat="server"  Text="Cancelar Requisicion" class="btn btn-default" />                
            </div> 
            <div class="form-group col-lg-2 col-md-2 col-sm-2" style="color:rgba(250, 184, 28, 0.50)">
                <asp:Button ID="btnGenerarOrden" runat="server"  Text="Generar Orden de Compra" class="btn btn-default" />                
            </div> 
        </div>
        <div>
            <iframe id="ifrOrdenCompra" src="" width="100%" height="500" style="border-width:0" runat="server"></iframe>
        </div> 
    
</asp:Content>
