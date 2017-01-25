<%@ Page Language="VB" AutoEventWireup="false" CodeFile="rptSalidasImportexMes.aspx.vb" Inherits="reportes_rptSalidasImportexMes" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <link href="../css/bootstrap.min.css" rel="stylesheet">
    <style>
        .container {
            width: 100%;
            
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
            <Triggers>
            <asp:PostBackTrigger ControlID="grvRequisiciones" />         
        </Triggers>

        <ContentTemplate>  
            <asp:Timer ID="Timer1" runat="server" Enabled="True" Interval="180000">
             </asp:Timer>  
            <table style="width:98%;">
                <tr>
                    <td colspan="2">
                        <h2><asp:Label ID="lblGrupo" runat="server">Total de Importe de Salidas de Almacén por mes del 
                                <%Dim fechaDesde As String = "01/01/" + cstr(DatePart(DateInterval.Year, Today)-1)
                                    If Request.QueryString("fechadesde") <> "" Then
                                        fechaDesde = Request.QueryString("fechadesde")
                                    End If
                                    Response.Write(fechaDesde)%> al 
                                <%Dim fechaHasta As String = Today
                                If Request.QueryString("fechahasta") <> "" Then
                                    fechaHasta = Request.QueryString("fechahasta")
                                    End If
                                    Response.Write(fechaHasta)%> </asp:Label>
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<input type="button" onclick="window.print();" id="btnPrint" style="background:url('../imagenes/print.png') no-repeat center; background-size:15px;height:30px; width: 30px;" class="btn btn-default" />                                                     
                        </h2>
                    </td>
                </tr>
                <tr>
                    <td style="width:100%;height:100%">
                        <div class="container">
                            
                            
                            <div>
                                <script type="text/javascript" src="https://www.google.com/jsapi"></script>
                                <div id="chart_div" style="height:380px"><div>
                                <script>
                                    google.load('visualization', '1', { packages: ['corechart', 'bar'] });
                                    google.setOnLoadCallback(drawMultSeries);

                                    function drawMultSeries() {
                                        var data = new google.visualization.DataTable();
                                        data.addColumn('string', 'Mes');
                                        data.addColumn('number', 'Anterior');
                                        data.addColumn('number', 'Actual');

                                        data.addRows([
                                        <%Dim x As Integer
                                    Dim arrLabel() As String = Session("label1").ToString.Split(",")
                                    Dim arrDato1() As String = Session("dato1").ToString.Split(",")
                                    Dim arrDato2() As String = Session("dato2").ToString.Split(",")
                                    For x = 0 To Session("total") - 1%> 
                                          ['<%=arrLabel(x)%>', <%=arrDato1(x)%>, <%=arrDato2(x)%>],
                                          <%Next%>                                          
                                        ]);

                                        var options = {
                                            title: '',
                                            hAxis: {
                                                title: 'Mes',
                                                format: 'h:mm a',
                                                viewWindow: {
                                                    min: [7, 30, 0],
                                                    max: [17, 30, 0]
                                                }
                                            },
                                            vAxis: {
                                                title: 'Total (escala de 1-12000 aprox)'
                                            }
                                        };

                                        var chart = new google.visualization.ColumnChart(
                                          document.getElementById('chart_div'));

                                        chart.draw(data, options);
                                    }
                            </script>
                            </div>
                        </div>  
                    </td>                     
                    <td style="vertical-align:central">
                        <asp:GridView ID="grvRequisiciones" class="table tbl-sel" runat="server" AutoGenerateColumns="false" 
                            ShowHeaderWhenEmpty="True">   
                            <HeaderStyle HorizontalAlign="Center" />                 
                           <RowStyle HorizontalAlign="center"/>                              
                            <Columns>
                            <asp:BoundField DataField="mes" HeaderText="MES"/> 
                            <asp:BoundField DataField="anterior" HeaderText="AÑO ANTERIOR"/>                           
                            <asp:BoundField DataField="actual" HeaderText="AÑO ACTUAL"/>   
                            <asp:BoundField DataField="diferencia" HeaderText=" DIFERENCIAS AÑO ANTERIOR"/> 
                            <asp:BoundField DataField="porcentaje" HeaderText="DIF % AÑO ANTERIOR"/>          
                            <asp:TemplateField>
                                <ItemTemplate>                          
                                <asp:ImageButton ID="AddButton" Width="15" runat="server" ImageUrl="../imagenes/der_up.png"
                                    CommandName="Seleccionar" CommandArgument="<%# CType(Container,GridViewRow).RowIndex %>" />                        
                                </ItemTemplate> 
                            </asp:TemplateField>                                   
                        </Columns>
                    </asp:GridView>                        
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                         
                    </td>
                </tr>
            </table>
            <div class="row" style="width:98%">
                <div class="col-md-1">
                </div>
                <div class="col-md-10">
                    <h2><h3>
                        <asp:Label ID="lblDepartamentos" runat="server">Total de Importe de Salidas de Almacén por Departamento del 
                                <%=Session("FechaDesdeDepto")%> al <%=Session("FechaHastaDepto")%> </asp:Label>
                        </h3>                           
                        </h2>
                </div>
                <div class="col-md-1">
                    
                </div>
            </div>
            <div class="row" style="width:98%">
                <div class="col-md-1">
                </div>
                <div class="col-md-10">
                    <asp:GridView ID="grvDepartamentos" class="table" runat="server" AutoGenerateColumns="false" 
                            ShowHeaderWhenEmpty="True">   
                            <HeaderStyle HorizontalAlign="Center" />                 
                           <RowStyle HorizontalAlign="center"/>                              
                            <Columns>
                            <asp:BoundField DataField="iddepartamento" HeaderText="MES" Visible="false"/> 
                            <asp:BoundField DataField="departamento" HeaderText="DEPARTAMENTO"/> 
                            <asp:BoundField DataField="salidas" HeaderText="TOTAL SALIDAS"/>                                 
                            <asp:BoundField DataField="actual" HeaderText="IMPORTE"/>   
                                      
                            <asp:TemplateField Visible="false">
                                <ItemTemplate>                          
                                <asp:ImageButton ID="AddButton" runat="server" Height="15" ImageUrl="../imagenes/der_up.png"
                                CommandName="Seleccionar" CommandArgument="<%# CType(Container,GridViewRow).RowIndex %>" />                        
                                </ItemTemplate> 
                            </asp:TemplateField>                                  
                        </Columns>
                    </asp:GridView>
                </div>
                <div class="col-md-1">
                    
                </div>
            </div>
            <div class="row" style="width:98%">
                <div class="col-md-8">
                </div>                    
                <div class="col-md-4">
                    <h2><h4>Información al 07/10/2015 8:30 a.m.</h4></h2>
                </div>                
            </div>            
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>
