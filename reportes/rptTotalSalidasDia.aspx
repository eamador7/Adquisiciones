<%@ Page Language="VB" AutoEventWireup="false" CodeFile="rptTotalSalidasDia.aspx.vb" Inherits="reportes_rptTotalSalidasDia" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Total de avance por Grupo</title>
    <!-- Bootstrap Core CSS -->
    <link href="../css/bootstrap.min.css" rel="stylesheet">
    <style>
        .container {
            width: 90%;
            /*margin: 20px auto;*/
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
            
            <table style="width:100%">
                <tr>
                    <td rowspan="2" style="width:80%;height:95%">
                        <div class="container">
                          <h2><asp:Label ID="lblGrupo" runat="server">Total de Salidas de Almacén por día del 
                              <%Dim fechaDesde As String = "01/01/" + cstr(DatePart(DateInterval.Year, Today)-1)
                                    If Request.QueryString("fechadesde") <> "" Then
                                        fechaDesde = Request.QueryString("fechadesde")
                                    End If
                                    Response.Write(fechaDesde)%> al 
                                <%Dim fechaHasta As String = Today
                                If Request.QueryString("fechahasta") <> "" Then
                                    fechaHasta = Request.QueryString("fechahasta")
                                    End If
                                    Response.Write(fechaHasta)%>  </asp:Label>  </h2>
                          <div>
                            <canvas id="canvas"></canvas>
                          </div>
                        </div>  
                    </td>
                    <td>
                        <input type="button" onclick="window.print();" id="btnPrint" style="background:url(../imagenes/print.png);background-repeat:no-repeat;background-size:15px;background-position:center;height:30px" class="btn btn-default" />                        
                    </td>
                </tr>
                <tr>
                    <td style="vertical-align:bottom">
                        <asp:GridView ID="grvRequisiciones" class="table tbl-sel" runat="server" AutoGenerateColumns="false" ShowHeaderWhenEmpty="True" Width="90%">   
                            <HeaderStyle HorizontalAlign="Center" />                 
                           <RowStyle HorizontalAlign="center"/>                              
                            <Columns>
                            <asp:BoundField DataField="fecha" HeaderText="FECHA"/>                            
                            <asp:BoundField DataField="total" HeaderText="TOTAL"/>                                               
                        </Columns>
                    </asp:GridView>
                    </td>
                </tr>
            </table>                                  
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
    <script src="//assets.codepen.io/assets/common/stopExecutionOnTimeout-6c99970ade81e43be51fa877be0f7600.js"></script>

            <script src='https://cdnjs.cloudflare.com/ajax/libs/Chart.js/1.0.2/Chart.min.js'></script>
            <script>
                
                var barChartData = {
                    labels: [<% =Session("label1")%>],
                    datasets: [{
                        fillColor: "rgba(0,60,0,1)",
                        strokeColor: "green",
                        data: [<% =Session("dato1")%>]
                    }]
                }

                var index = 11;
                var ctx = document.getElementById("canvas").getContext("2d");
                var barChartDemo = new Chart(ctx).Bar(barChartData, {
                    responsive: true,
                    barValueSpacing: 2
                });

                var dato2 = [<%=Session("dato2")%>]
                var label2 = [<%=Session("label2")%>]
                <%If Session("total") > 10 Then
                    Dim x As Integer
                    For x = 0 To (Session("total") + (Session("total") - 11))%>                                
                setTimeout(function () {
                    barChartDemo.removeData();
                    barChartDemo.addData([dato2[<%=x%>]], label2[<%=x%>]);                    
                }, 10000*<%=(x+1)%>);
                <%Next
                  End If%>
                
            </script>
</body>
</html>
