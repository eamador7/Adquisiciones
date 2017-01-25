<%@ Page Language="VB" AutoEventWireup="false" CodeFile="rptSalidasDepartamento.aspx.vb" Inherits="reportes_rptSalidasDepartamento" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Total de avance por Grupo</title>
    <!-- Bootstrap Core CSS -->
    <link href="css/bootstrap.min.css" rel="stylesheet">
    <style>
        .container {
            width: 90%;
            margin: 20px auto;
        }
        .p {
            text-align: center;
            font-size: 14px;
            padding-top: 140px;
        }
        .tbl-sel tbody tr:hover {
	    background-color: lightgreen;
	    cursor: pointer;
		}        	
		.tbl-btn:hover {
			color: #993300;
		}
        th,tr {text-align:center}
    </style>

</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="True"></asp:ScriptManager>    
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>  
            <asp:Timer ID="Timer1" runat="server" Enabled="True" Interval="160000">
             </asp:Timer>  
            <asp:HiddenField ID="hdnDependencia" runat="server" />
            <header class="codrops-header" style="background:rgba(60, 62, 68, 0.9);font-family:'AR JULIAN'; position:fixed; top:0px; width:100%;">                                                
				<table style="width:100%">
                    <tr>
                        <td style="width:20%"></td>
                        <td style="width:60%;color:white"> <h1>ALMACEN</h1></td>
                        <td style="width:20%"><asp:ImageButton ID="btnActualizar" runat="server" Height="30px" ImageUrl="images/refresh.png" class="btn btn-default" />
                        &nbsp;&nbsp;&nbsp;<asp:ImageButton ID="btnSalir" runat="server" Height="30px" ImageUrl="images/exit.png" class="btn btn-default" /></td>
                    </tr>
				</table>                                                                                                             
			</header>
            <table style="background:rgba(0, 0, 0, 0.01); height:50px"><tr><td>&nbsp;</td></tr></table>
            <div style="box-shadow:inset 0px 10px 10px rgba(0,0,0,0.3);top:68px;position:fixed;width:100%;">&nbsp;</div>
            <table style="width:100%">
                <tr>
                    <td style="width:80%;height:90%">
                        <div class="container">
                          <h2><asp:Label ID="lblGrupo" runat="server">Total de importes por Departamento</asp:Label>  </h2>
                          <div>
                            <canvas id="canvas"></canvas>
                          </div>
                        </div>  
                    </td>
                    <td style="vertical-align:bottom">
                        <asp:GridView ID="grvDependencia" class="table tbl-sel" runat="server" AutoGenerateColumns="false" ShowHeaderWhenEmpty="True" Width="90%">   
                            <HeaderStyle HorizontalAlign="Center" />                 
                           <RowStyle HorizontalAlign="center"/>                              
                            <Columns>
                            <asp:BoundField DataField="dependencia" HeaderText="  DEPENDENCIA"/>                            
                            <asp:BoundField DataField="votaron" HeaderText="TOTAL"/>                      
                            <asp:TemplateField>
                              <ItemTemplate>                          
                                <asp:ImageButton ID="AddButton" runat="server" Height="15" ImageUrl="images/der_up.png"
                                  CommandName="Actualizar" CommandArgument="<%# CType(Container,GridViewRow).RowIndex %>" />                        
                              </ItemTemplate> 
                            </asp:TemplateField>                    
                        </Columns>
                    </asp:GridView>
                    </td>
                </tr>
            </table>
                      
            <script src="//assets.codepen.io/assets/common/stopExecutionOnTimeout-6c99970ade81e43be51fa877be0f7600.js"></script>

            <script src='https://cdnjs.cloudflare.com/ajax/libs/Chart.js/1.0.2/Chart.min.js'></script>
            <script>
                
                var barChartData = {
                    labels: ['DEPTO PLANTA TRATAMIENTO NORTE',
                            'DEPTO RECURSOS HUM            ',
                            'DEPTO. AGUA RECUPERADA        ',
                            'DEPTO. BIENES PATRIMONIALES   ',
                            'DEPTO. CENTRO DE INFORM.Y SERV',
                            'DEPTO. CONTABILIDAD Y PRESUP. ',
                            'DEPTO. CULTURA DEL AGUA       ',
                            'DEPTO. DE ADQUISICIONES       ',
                            'DEPTO. DE ATENCION CIUDADANA  ',
                            'DEPTO. DE EVALUAC.Y SEGUIM.   '],
                    datasets: [{
                        fillColor: "rgba(0,60,0,1)",
                        strokeColor: "green",
                        data: [988900.27,
                                759183.99,
                                223677.09,
                                118953.46,
                                11002.28,
                                54323.05,
                                198605.51,
                                3114672.41,
                                41183.43,
                                66825.04]
                    }]
                }

                var index = 11;
                var ctx = document.getElementById("canvas").getContext("2d");
                var barChartDemo = new Chart(ctx).Bar(barChartData, {
                    responsive: true,
                    barValueSpacing: 2
                });

                var dato2 = [180544.91,
                            14102.56,
                            33131.49,
                            284231.61,
                            2637529.26,
                            4970434.36,
                            153498.45,
                            3278928.27,
                            589611.99,
                            4032.67,
                            2043.21,
                            5539607.12,
                            38252.26,
                            8376.44,
                            12945.72]
                var label2 = ['DEPTO. DE GESTORIA CIUDADANA  ',
                                'DEPTO. DE OPERACION SUCURSALES',
                                'DEPTO. FACTURACION Y COBRANZA ',
                                'DEPTO. PLANEACION E INGENIERIA',
                                'DEPTO. RED ALCANTARILLADO     ',
                                'DEPTO. RED HIDRAULICA MANT.   ',
                                'DEPTO. SISTEMAS               ',
                                'DEPTO. SUMINISTRO             ',
                                'DEPTO. SUPERVISION Y CONSTRUC.',
                                'DIRECCION ADMINISTRATIVA      ',
                                'DIRECCION ATENCION COMUNITARIA',
                                'DIRECCION COMERCIAL           ',
                                'DIRECCION COMERCIAL AGUA TRAT.',
                                'DIRECCION JURIDICA            ',
                                'DIRECCION TECNICA             ']
                <%Dim x As Integer                
                For x = 0 To 23%>
                setTimeout(function () {
                    barChartDemo.removeData();
                    barChartDemo.addData([dato2[<%=x%>]], label2[<%=x%>]);                    
                }, 10000*<%=(x+1)%>);
                <%Next%>
                
            </script>
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>
