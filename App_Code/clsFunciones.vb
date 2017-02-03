Imports Microsoft.VisualBasic
Imports System.Data
Imports System.Data.SqlClient
Imports System.Net.Mail.MailMessage
Public Class clsFunciones
    Dim query As New SqlCommand
    Dim ResQuery As SqlDataReader
    Dim ResQueryDataset As New DataSet
    Dim QueryString, mensaje As String
    Dim ResInt As Integer
    Public con As New clsConexion

    Public Function Llena_Dataset(ByVal Conexion As String, ByRef DS As DataSet, ByVal Nombre_Tabla As String, _
            ByVal Nombre_Procedimiento As String, ByVal Parametros As String, ByVal Nombre_Parametros As String, _
            ByVal Param_Tabla As DataTable) As String
        Try
            mensaje = con.Open(Conexion)
            If mensaje = "OK" Then
                Dim arrNobreParametros, arrParametros As String()
                Dim comProcedimiento As New SqlClient.SqlCommand
                comProcedimiento.CommandType = CommandType.StoredProcedure
                comProcedimiento.CommandText = Nombre_Procedimiento
                comProcedimiento.Connection = con.conexionsql
                If Parametros <> "" Then
                    Dim i As Integer
                    arrNobreParametros = Nombre_Parametros.Split(",")
                    For i = 0 To arrNobreParametros.Length - 1
                        arrParametros = Parametros.Split(",")
                        If IsNumeric(arrParametros(i)) Then
                            comProcedimiento.Parameters.AddWithValue(arrNobreParametros(i), CDbl(arrParametros(i)))
                        Else
                            comProcedimiento.Parameters.AddWithValue(arrNobreParametros(i), arrParametros(i).Replace("-", ",").Replace("*", ","))
                        End If
                    Next
                End If
                If Not Param_Tabla Is Nothing Then
                    If Param_Tabla.Rows.Count > 0 Then
                        comProcedimiento.Parameters.AddWithValue("@tabla", Param_Tabla)
                    End If
                End If


                Dim dadTabla As New SqlClient.SqlDataAdapter(comProcedimiento)
                comProcedimiento = Nothing
                Try
                    DS.Tables(Nombre_Tabla).Clear()
                Catch ex As Exception

                End Try
                dadTabla.Fill(DS, Nombre_Tabla)
                dadTabla = Nothing
            End If
            Return mensaje
        Catch ex As Exception
            Return ex.Message
        End Try
    End Function

    Public Function EjecutaProcedimiento(ByVal Conexion As String, ByVal Nombre_Procedimiento As String,
                                         ByVal Parametros As String, ByVal Nombre_Parametros As String) As String
        Try
            mensaje = con.Open(Conexion)
            If mensaje = "OK" Then
                Dim arrNobreParametros, arrParametros As String()
                Dim comProcedimiento As New SqlClient.SqlCommand
                comProcedimiento.CommandType = CommandType.StoredProcedure
                comProcedimiento.CommandText = Nombre_Procedimiento
                comProcedimiento.Connection = con.conexionsql
                If Parametros <> "" Then
                    Dim i As Integer
                    arrNobreParametros = Nombre_Parametros.Split(",")
                    For i = 0 To arrNobreParametros.Length - 1
                        arrParametros = Parametros.Split(",")
                        If IsNumeric(arrParametros(i)) Then
                            comProcedimiento.Parameters.AddWithValue(arrNobreParametros(i), CDbl(arrParametros(i)))

                        Else
                            comProcedimiento.Parameters.AddWithValue(arrNobreParametros(i), arrParametros(i).Replace("-", "").Replace("*", ",").Replace("|", ",").Replace("#", ""))
                        End If
                    Next
                End If
                EjecutaProcedimiento = comProcedimiento.ExecuteScalar
            End If
        Catch ex As Exception
            Return ex.Message
        End Try
    End Function

    Function LlenarDropDownList(ByRef objeto As Object, ByVal Conexion As String, ByRef DS As DataSet,
                                ByVal id_campo As String, ByVal desc_campo As String,
                                ByVal Nombre_Tabla As String, ByVal Nombre_Procedimiento As String,
                                ByVal Parametros As String, ByVal Nombre_Parametros As String) As String
        Try
            mensaje = con.Open(Conexion)
            If mensaje = "OK" Then
                Dim arrNobreParametros, arrParametros As String()
                Dim comProcedimiento As New SqlClient.SqlCommand
                comProcedimiento.CommandType = CommandType.StoredProcedure
                comProcedimiento.CommandText = Nombre_Procedimiento
                comProcedimiento.Connection = con.conexionsql
                If Parametros <> "" Then
                    Dim i As Integer
                    arrNobreParametros = Nombre_Parametros.Split(",")
                    For i = 0 To arrNobreParametros.Length - 1
                        arrParametros = Parametros.Split(",")
                        If IsNumeric(arrParametros(i)) Then
                            comProcedimiento.Parameters.AddWithValue(arrNobreParametros(i), CDbl(arrParametros(i)))
                        Else
                            comProcedimiento.Parameters.AddWithValue(arrNobreParametros(i), arrParametros(i).Replace("-", ",").Replace("*", ",").Replace("#", ""))
                        End If
                    Next
                End If
                Dim dadTabla As New SqlClient.SqlDataAdapter(comProcedimiento)
                comProcedimiento = Nothing
                Try
                    DS.Tables(Nombre_Tabla).Clear()
                Catch ex As Exception
                End Try
                dadTabla.Fill(DS, Nombre_Tabla)
                dadTabla = Nothing
                Dim y As Integer
                For y = 0 To DS.Tables(Nombre_Tabla).Rows.Count - 1
                    objeto.Items.Add(New ListItem(DS.Tables(Nombre_Tabla).Rows(y).Item(desc_campo), DS.Tables(Nombre_Tabla).Rows(y).Item(id_campo).ToString))
                Next
            End If
            Return mensaje
        Catch ex As Exception
            Return ex.Message
        End Try
    End Function

    Public Function Llena_Grid(ByVal objeto As Object, ByVal Conexion As String, ByRef DS As DataSet,
                          ByVal Nombre_Tabla As String, ByVal Nombre_Procedimiento As String,
                          ByVal Parametros As String, ByVal Nombre_Parametros As String) As String
        Try
            mensaje = con.Open(Conexion)
            If mensaje = "OK" Then
                Dim arrNobreParametros, arrParametros As String()
                Dim comProcedimiento As New SqlClient.SqlCommand
                comProcedimiento.CommandType = CommandType.StoredProcedure
                comProcedimiento.CommandText = Nombre_Procedimiento
                comProcedimiento.Connection = con.conexionsql
                If Parametros <> "" Then
                    Dim i As Integer
                    arrNobreParametros = Nombre_Parametros.Split(",")
                    For i = 0 To arrNobreParametros.Length - 1
                        arrParametros = Parametros.Split(",")
                        If IsNumeric(arrParametros(i)) Then
                            comProcedimiento.Parameters.AddWithValue(arrNobreParametros(i), CDbl(arrParametros(i)))
                        Else
                            comProcedimiento.Parameters.AddWithValue(arrNobreParametros(i), arrParametros(i).Replace("-", ",").Replace("*", ",").Replace("#", ""))
                        End If
                    Next
                End If
                Dim dadTabla As New SqlClient.SqlDataAdapter(comProcedimiento)
                comProcedimiento = Nothing
                Try
                    DS.Tables(Nombre_Tabla).Clear()
                Catch ex As Exception
                End Try
                dadTabla.Fill(DS, Nombre_Tabla)
                dadTabla = Nothing
                objeto.DataSource = DS.Tables(Nombre_Tabla)
                'objeto.Columns(0).Visible = False
            End If
            Return mensaje
        Catch ex As Exception
            Return ex.Message
        End Try
    End Function

    Public Function ConsultaInsert(ByVal QueryString As String, ByVal Conexion As String) As String
        Try
            mensaje = con.Open(Conexion)
            If mensaje = "OK" Then
                query.Connection = con.conexionsql 'conexionsql
                query.CommandText = QueryString
                query.ExecuteNonQuery()
                Return "OK"
            Else
                Return mensaje
            End If
        Catch ex As Exception
            Return ex.Message
        End Try
    End Function

    Public Function ConsultaSelect(ByVal QueryString As String, ByVal Conexion As String) As SqlDataReader
        Try
            mensaje = con.Open(Conexion)
            query.Connection = con.conexionsql
            query.CommandText = QueryString
            ResQuery = query.ExecuteReader
            Return ResQuery
        Catch ex As Exception
            Return Nothing
        End Try
    End Function

    Public Function ConsultaDataset(ByVal QueryString As String, ByVal Tabla As String, ByVal Conexion As String) As DataSet
        Try
            mensaje = con.Open(Conexion)
            If mensaje = "OK" Then
                Dim ResQueryDataAdapter As New SqlDataAdapter(QueryString, con.conexionsql)
                ResQueryDataAdapter.Fill(ResQueryDataset, Tabla)
            End If
            Return ResQueryDataset
        Catch ex As Exception
            Return Nothing
        End Try
    End Function

    Public Shared Function EnviarCorreo(ByVal Mensaje As String, ByVal EMail As String, Optional ByVal subject As String = "JMAS") As String
        Try
            'MsgBox(EMail + vbNewLine + ArchivoXML)
            Dim _Message As New System.Net.Mail.MailMessage()
            Dim _SMTP As New System.Net.Mail.SmtpClient
            'CONFIGURACIÓN DEL STMP

            _SMTP.Credentials = New System.Net.NetworkCredential("extranet@jmaschihuahua.gob.mx", "Cuidaelagu@96")
            '_SMTP.Host = "mail.jmaschihuahua.gob.mx"
            _SMTP.Host = "74.220.207.110"
            _SMTP.Port = 25
            _SMTP.EnableSsl = False
            ''_SMTP.UseDefaultCredentials = True

            ' CONFIGURACION DEL MENSAJE 
            _Message.[To].Add(EMail)
            '_Message.[To].Add("hcedillo78@hotmail.com")

            'Cuenta de Correo al que se le quiere enviar el e-mail 
            _Message.From = New System.Net.Mail.MailAddress("extranet@jmaschihuahua.gob.mx", "Extranet", System.Text.Encoding.UTF8)
            'Quien lo envía 
            _Message.Subject = subject
            'Sujeto del e-mail 
            _Message.SubjectEncoding = System.Text.Encoding.UTF8
            'Codificacion 
            _Message.Body = Mensaje
            'contenido del mail 
            _Message.BodyEncoding = System.Text.Encoding.UTF8
            _Message.Priority = System.Net.Mail.MailPriority.High
            _Message.IsBodyHtml = False
            'ADICION DE DATOS ADJUNTOS 
            ''Dim _File As String = My.Application.Info.DirectoryPath & Archivo
            'Dim _FileXML As String = ArchivoXML
            'archivo que se quiere adjuntar 
            'Dim _AttachmentXML As New System.Net.Mail.Attachment(_FileXML, System.Net.Mime.MediaTypeNames.Application.Octet)
            '_Message.Attachments.Add(_AttachmentXML)
            'Dim _FilePDF As String = ArchivoPDF
            'archivo que se quiere adjuntar 
            'Dim _AttachmentPDF As New System.Net.Mail.Attachment(_FilePDF, System.Net.Mime.MediaTypeNames.Application.Octet)
            '_Message.Attachments.Add(_AttachmentPDF)

            'ENVIO 
            Try
                _SMTP.Send(_Message)
                _SMTP = Nothing
                _Message = Nothing
                Return "OK"
            Catch ex As System.Net.Mail.SmtpException
                Return "Error al enviar el correo de confirmacion:" & ex.Message

            End Try
        Catch ex As Exception
            Return "Error al enviar el correo de confirmacion:" & ex.Message
        End Try
    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="strMensaje">Cualquier texto</param>
    ''' <param name="mySender">Siempre "Me"</param>
    ''' <param name="posicion">1. top 2. topLeft 3.topCenter 4. topRight
    ''' 5. center 6. centerLeft 7. centerRight 8. bottom 9. bottomLeft 10. bottomCenter 11.bottomRight
    ''' </param>
    ''' <param name="tipo">1. alert 2. success 3. warning 4.error 5.information</param>
    Public Shared Sub Alert(ByVal strMensaje As String, ByRef mySender As Page, Optional ByVal posicion As Integer = 1, Optional ByVal tipo As Integer = 1)
        Dim messageType As String = ""
        Dim messagePosition As String = ""
        Select Case posicion
            Case 1
                messagePosition = "top"
            Case 2
                messagePosition = "topLeft"
            Case 3
                messagePosition = "topCenter"
            Case 4
                messagePosition = "topRight"
            Case 5
                messagePosition = "center"
            Case 6
                messagePosition = "centerLeft"
            Case 7
                messagePosition = "centerRight"
            Case 8
                messagePosition = "bottom"
            Case 9
                messagePosition = "bottomLeft"
            Case 10
                messagePosition = "bottomCenter"
            Case 11
                messagePosition = "bottomRight"
            Case Else
                messagePosition = "top"

        End Select

        Select Case tipo
            Case 1
                messageType = "alert"
            Case 2
                messageType = "success"
            Case 3
                messageType = "warning"
            Case 4
                messageType = "error"
            Case 5
                messageType = "information"
            Case Else
                messageType = "alert"
        End Select
        ScriptManager.RegisterStartupScript(mySender, mySender.GetType(), "script234", "noty({text: '" & strMensaje & "', layout: '" & messagePosition & "', closeWith: ['click', 'hover'], timeout: [3000], type: '" & messageType & "'});", True)
    End Sub


End Class
