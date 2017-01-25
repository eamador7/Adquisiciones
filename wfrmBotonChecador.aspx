<%@ Page Language="VB" MasterPageFile="MPCompras.master" AutoEventWireup="false" CodeFile="wfrmBotonChecador.aspx.vb" Inherits="wfrmBotonChecador" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

</asp:Content>
    
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">  
   <asp:Timer ID="Timer1" runat="server" Enabled="false" Interval="2000"></asp:Timer> 
              <div class="row">  </div>
    
          
                        <div class="row">    
                             <div class=" col-lg-5 col-md-5 col-sm-5">  </div>
                        
                                       
                             <div class="col-lg-7 col-md-7 col-sm-7">
                                 <asp:ImageButton id="imagebutton1" runat="server"
                                   AlternateText="ImageButton 1"
                                   ImageAlign="left"
                                   ImageUrl="imagenes/timer-icon.png"
                                     cssclass="center-block"
                                   />
                              
                             </div>
                                                
                             
    
</asp:content>
