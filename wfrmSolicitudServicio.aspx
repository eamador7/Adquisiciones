<%@ Page Language="VB" MasterPageFile="MPCompras.master"  AutoEventWireup="false" CodeFile="wfrmSolicitudServicio.aspx.vb" Inherits="wfrmSolicitudServicio" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link rel="stylesheet" href="http://localhost:1741/code.jquery.com/ui/1.11.4/themes/smoothness/jquery-ui.css"/>
    <script src="//code.jquery.com/jquery-1.10.2.js"></script>
    <script src="//code.jquery.com/ui/1.11.4/jquery-ui.js"></script>

    <script>
        function fnBuscaEmpleado()
        {
            window.location.assign('/almacen/wfrmBuscaEmpleado.aspx')
        }
	</script>
    <style>
        .tbl-txt {
            font-size:.7em;
        }
	</style>
</asp:Content>
    
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">   

    <asp:Timer ID="Timer1" runat="server" Enabled="false" Interval="3000"></asp:Timer>
    <div class="panel panel-success">
        <div class="panel-body">
            <div class="row">
                <div class="form-group col-lg-2 col-md-2 col-sm-2">                
                    <asp:label ID="Label2" runat="server">NO. EMPLEADO:</asp:label>               
                    <asp:TextBox ID="txtNompleado" ReadOnly="true" class="form-control" runat="server" ToolTip="No. Empleado" placeholder="No. Empleado"></asp:TextBox>                 
                </div> 
                <div class="form-group col-lg-4 col-md-4 col-sm-4">                
                    <asp:label ID="lblSolicitado" runat="server">SOLICITADO POR:</asp:label>
                    <asp:TextBox ID="txtSolictado" ReadOnly="true" class="form-control" runat="server" ToolTip="Solicitado por" placeholder="Solicitado por"></asp:TextBox>
                </div>  
                                                                       
            </div>
            <div class="row">                    
                <div class="form-group col-lg-3 col-md-3 col-sm-3">                
                    <asp:label ID="lblFecha" runat="server">FECHA:</asp:label>               
                    <asp:TextBox ID="txtFecha" ReadOnly="true" class="form-control" runat="server" ToolTip="Fecha" placeholder="Fecha"></asp:TextBox>                 
                </div>  
            </div> 
          
            <div class="row">   
                <!--                 
                <div class="form-group col-lg-6 col-md-6 col-sm-6">
                    <asp:label ID="Label1" runat="server">CUENTA CONTABLE:</asp:label>  
                    <asp:TextBox ID="txtCuentaContable" ReadOnly="true" class="form-control" runat="server" ToolTip="Cuenta Contable" placeholder="Cuenta Contable"></asp:TextBox> 
                </div> -->                
 
                <div class="form-group col-lg-6 col-md-6 col-sm-6">
                    <asp:label ID="lblServicio" runat="server">SERVICIO SOLICITADO:</asp:label>  
                    <asp:TextBox ID="txtServicio" TextMode="MultiLine" class="form-control" runat="server" ToolTip="Servicio solicitado" placeholder="Servicio solicitado"></asp:TextBox>                
                </div>
            </div>                           
            <div class="row">
            </div>            
            
        </div>
    </div>  
    <div class="panel panel-success">
        <div class="row">
            <div class="col-md-12">
            </div>
        </div>                                         
    </div>
    <div class="row panel-body" style="color:black">
        <div class="form-group col-lg-10 col-md-10 col-sm-10">
                                                         
        </div>
        <div class="form-group col-lg-2 col-md-2 col-sm-2" style="color:rgba(250, 184, 28, 0.50)">
            <asp:Button ID="btnSolicitar" runat="server"  Text="Solicitar servicio" class="btn btn-default" />                
        </div> 
    </div>
 <script>
     document.getElementById('ContentPlaceHolder1_btnSolicitars').class = 'btn btn-default';
	</script>
</asp:content>
