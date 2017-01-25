<%@ Page Language="VB" MasterPageFile="MPCompras.master" AutoEventWireup="false" CodeFile="wfrmAnexoComiteAdquisiciones.aspx.vb" Inherits="wfrmAnexoComiteAdquisiciones" %>
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
							<asp:BoundField DataFormatString="{0:C2}" DataField="total" HeaderText="TOTAL REQ."/>
							<asp:BoundField DataField="departamento" HeaderText="REQUIRENTE"/>                         
							<asp:BoundField DataField="concepto" HeaderText="CONCEPTO"/>    
							<asp:BoundField DataField="nombreProveedor" HeaderText="PROVEEDOR"/>    
							<asp:BoundField DataFormatString="{0:N2}" DataField="cantidad" HeaderText="CANTIDAD"/>    
							<asp:BoundField DataField="precio" DataFormatString="{0:C2}" HeaderText="IMPORTE"/>    
							<%--<asp:BoundField DataField="fecha" HeaderText="FECHA"/>  --%> 
							<asp:BoundField DataField="contrato" HeaderText="Contrato"/>    

							
				<%--			<asp:TemplateField>
								<ItemTemplate>                          
								<asp:ImageButton ID="AddButton" runat="server" Height="15" ImageUrl="imagenes/der_up.png"
								CommandName="Seleccionar" CommandArgument="<%# CType(Container,GridViewRow).RowIndex %>" />                        
								</ItemTemplate> 
							</asp:TemplateField>--%>
					
						</Columns>
					</asp:GridView> 
			   
				</div>
			</div>
		  </div>
			<hr />
		<div class="row panel-body" style="color:black">
			<div class="form-group col-lg-6 col-md-6 col-sm-6">
	   
			</div>
				<div class="form-group col-lg-2 col-md-2 col-sm-5">
						
					</div>  
			 <div class="form-group col-lg-2 col-md-2 col-sm-2" style="color:rgba(250, 184, 28, 0.50)">
	   
			</div> 
			<div class="form-group col-lg-2 col-md-2 col-sm-2" style="color:rgba(250, 184, 28, 0.50)">
				<asp:Button ID="btnImprimir" runat="server"  Text="Imprimir Anexo" class="btn btn-default" />                
			</div> 
		</div>
			
		<iframe id="ifrImpresion" src="" width="100%" height="500" style="border-width:0" runat="server"></iframe>
		

		 
		</div> 
	

</asp:Content>
