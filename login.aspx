<%@ Page Language="VB" AutoEventWireup="false" CodeFile="login.aspx.vb" Inherits="login" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link rel="stylesheet" type="text/css" href="css/acceso.css" />
</head>
<body>
    <div class="login">
  <div class="heading">
    <h2>Acceso</h2>
    <form id="form1" runat="server" action="login.aspx">

      <div class="input-group input-group-lg">
        <span class="input-group-addon"><i class="fa fa-user"></i></span>
        <input type="text" name="usr" class="form-control" placeholder="Usuario">
          </div>

        <div class="input-group input-group-lg">
          <span class="input-group-addon"><i class="fa fa-lock"></i></span>
          <input type="password" name="pass" class="form-control" placeholder="Contraseña">
        </div>

        <button type="submit" class="float">Entrar</button>
       </form>
 		</div>
 </div>
</body>
</html>
