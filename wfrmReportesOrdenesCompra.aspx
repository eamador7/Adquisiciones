<%@ Page Language="VB" MasterPageFile="MPCompras.master" AutoEventWireup="false" CodeFile="wfrmReportesOrdenesCompra.aspx.vb" Inherits="wfrmReportesOrdenesCompra" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script>
       
	</script>
    <style>
        .tbl-txt {
            font-size:.7em;
        }
	</style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">    
    <div class="panel panel-success">
        <div class="panel-body">
            <div class="row">
                <div class="form-group col-lg-10 col-md-10 col-sm-10">
                    <div class="form-group col-lg-4 col-md-4 col-sm-4">
                        <asp:label ID="Label1" runat="server">REQUISICION:</asp:label>
                        <asp:TextBox ID="txtRequisicion" TextMode="Number" class="form-control" runat="server" ToolTip="Requisicion" placeholder="Requisicion"></asp:TextBox>                
                    </div>
                    <div class="form-group col-lg-4 col-md-4 col-sm-4">
                        <asp:label ID="lblFechaDesde" runat="server">FECHA DESDE:</asp:label>
                        <asp:TextBox ID="txtFechaDesde" TextMode="Date" class="form-control" runat="server" ToolTip="dd/mm/aaaa" placeholder="dd/mm/aaaa"></asp:TextBox>                            
                    </div>  
                    <div class="form-group col-lg-4 col-md-4 col-sm-4">
                        <asp:label ID="lblFechaHasta" runat="server">FECHA HASTA: </asp:label>
                        <asp:TextBox ID="txtFechaHasta" TextMode="Date" class="form-control" runat="server" ToolTip="dd/mm/aaaa" placeholder="dd/mm/aaaa"></asp:TextBox>                
                    </div>
                    <div class="form-group col-lg-6 col-md-6 col-sm-6"> 
                        <asp:label ID="lblEstatus" runat="server">REPORTE:</asp:label>               
                        <asp:DropDownList ID="ddlReporte" runat="server" class="form-control">                            
                        </asp:DropDownList>                
                    </div>                
                    <div class="form-group col-lg-6 col-md-6 col-sm-6">                
                        <asp:label ID="lblSolicitado" runat="server">DEPARTAMENTO:</asp:label>
                        <asp:DropDownList ID="ddlDepartamento" runat="server" class="form-control">                            
                        </asp:DropDownList> 
                    </div>                        
                </div>
                <div class="form-group col-lg-2 col-md-2 col-sm-2" style="color:rgba(250, 184, 28, 0.50)">  
                    <br />                     
                    <asp:Button ID="btnConsultar" runat="server"  Text="Consultar" class="btn btn-default" />  
                    
                </div>
            </div>
        </div>            
    </div> 
     <div>
        <iframe id="ifrOrdenSalida" src="" width="100%" height="500" style="border-width:0" runat="server"></iframe>
    </div> 
</asp:content>