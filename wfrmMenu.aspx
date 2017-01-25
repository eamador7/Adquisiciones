<%@ Page Language="VB" AutoEventWireup="false" CodeFile="wfrmMenu.aspx.vb" Inherits="wfrmMenu" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>JMAS Chihuahua</title>
    
    <link rel="stylesheet" type="text/css" href="css/normalize.css" />
	<link rel="stylesheet" type="text/css" href="css/demo.css" />
	<link rel="stylesheet" type="text/css" href="css/icons.css" />
	<link rel="stylesheet" type="text/css" href="css/component.css" />
	<script src="js/modernizr.custom.js"></script>
    <script>
        function fnCambiaOpcion(url,titulo)
        {            
            document.getElementById('lblTitulo').innerHTML = titulo
            document.getElementById('ifrContenido').src = url
        }
        
    </script>
 </head>
<body>   
    <div> 
        <div class="container" style="width:100%">
			<!-- Push Wrapper -->
			<div class="mp-pusher" id="mp-pusher">

				<!-- mp-menu -->
				<nav id="mp-menu" class="mp-menu">
					<div class="mp-level">
						<h2 class="icon icon-world">JMAS</h2>
						<ul>
                            <%Dim i As Integer
                                For i = 0 To Session("0").rows.count - 1%>
                             <li class="icon icon-arrow-left">
								<a class="icon icon-news" href="#"><%=Session("0").rows(i).item(1)%></a>

								<div class="mp-level">
									<h2 class="icon icon-news"><%=Session("0").rows(i).item(1)%></h2>
                                    <%Dim y As Integer
                                        For y = 0 To Session("" & Session("0").rows(i).item(0) & "").Rows.Count - 1%>
                                        <ul>
										    <li class="icon icon-arrow-left">
											    <a class="icon icon-shop" href="#"><%=Session("" & Session("0").rows(i).item(0) & "").Rows(y).item(1)%></a>

											    <div class="mp-level">
												    <h2><%=Session("" & Session("0").rows(i).item(0) & "").Rows(y).item(1)%></h2>
												    <ul>
                                                        <%If Not Session("" & Session("" & Session("0").rows(i).item(0) & "").rows(y).item(0) & "") Is Nothing Then
                                                                Dim x As Integer
                                                                For x = 0 To Session("" & Session("" & Session("0").rows(i).item(0) & "").rows(y).item(0) & "").rows.Count - 1%>
													    <li><a onclick="fnCambiaOpcion('<%=Session("" & Session("" & Session("0").rows(i).item(0) & "").rows(y).item(0) & "").rows(x).item(2)%>','<%=Session("" & Session("" & Session("0").rows(i).item(0) & "").rows(y).item(0) & "").rows(x).item(1)%>')"><%=Session("" & Session("" & Session("0").rows(i).item(0) & "").rows(y).item(0) & "").rows(x).item(1)%></a></li>
												                <%Next
												                                                     End If%>
                                                    </ul>
											    </div>
										    </li>										    								
									    </ul>    
                                        <%Next%>
									
								</div>
							</li>                             
                            <%Next%>							
                            <li class="icon icon-arrow-left">
								<a class="icon icon-lock" href="logout.aspx">Cerrar Sesión</a>
							</li>							
						</ul>
					</div>
				</nav>
				<!-- /mp-menu -->

				<div class="scroller"><!-- this is for emulating position fixed of the nav -->
					<div class="scroller-inner" style="background:url(imagenes/jmasHorizontalChico.jpg); background-repeat:no-repeat; background-position:center">
						<!-- Top Navigation -->						
						<header class="codrops-header">
							<div class="block" style="text-align:left; background-image:url(imagenes/jmasHorizontalChico.jpg); background-size:95px; background-repeat:no-repeat; background-position:right">
								<table style="width:100%;color:#3c763d">
                                    <tr>
                                        <td style="width:23%"><p><a href="#" id="trigger" class="menu-trigger"></a></p></td>
                                        <td style="text-align:center"><h2><label id="lblTitulo"></label></h2></td>
                                        <td style="width:23%">Usuario: <b><%=Session("usuario").ToString.ToUpper%></b></td>
                                    </tr>
								</table>                                								
							</div>                            
						</header>
                        <iframe id="ifrContenido" width="100%"></iframe>
                    </div><!-- /scroller-inner -->
				</div><!-- /scroller -->
			</div><!-- /pusher -->
		</div><!-- /container -->
		<script src="js/classie.js"></script>
		<script src="js/mlpushmenu.js"></script>
		<script>
		    new mlPushMenu(document.getElementById('mp-menu'), document.getElementById('trigger'));
		    document.getElementById('ifrContenido').height = screen.availHeight - 250;
            
		</script>
		
   </div>
</body>
</html>
