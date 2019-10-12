﻿<%@ WebHandler Language="VB" Class="DemoPrintFileWithPwdProtectionHandler" %>

Imports System
Imports System.Web

Imports Neodynamic.SDK.Web

Public Class DemoPrintFileWithPwdProtectionHandler : Implements IHttpHandler


    '############### IMPORTANT!!! ############
    ' If your website requires AUTHENTICATION, then you MUST configure THIS Handler file
    ' to be ANONYMOUS access allowed!!!
    '######################################### 


    Public Sub ProcessRequest(ByVal context As HttpContext) Implements IHttpHandler.ProcessRequest

        If WebClientPrint.ProcessPrintJob(context.Request.Url.Query) Then
            Dim useDefaultPrinter As Boolean = (context.Request("useDefaultPrinter") = "checked")
            Dim printerName As String = context.Server.UrlDecode(context.Request("printerName"))

            Dim fileType As String = context.Request("filetype")

            Dim fileName As String = (Guid.NewGuid().ToString("N") & ".") + fileType
            Dim filePath As String = Nothing
            Select Case fileType
                Case "PDF"
                    filePath = "~/files/LoremIpsum-PasswordProtected.pdf"
                    Exit Select
                Case "DOC"
                    filePath = "~/files/LoremIpsum-PasswordProtected.doc"
                    Exit Select
                Case "XLS"
                    filePath = "~/files/SampleSheet-PasswordProtected.xls"
                    Exit Select

            End Select

            If filePath IsNot Nothing Then

                'get and set the RSA pub key generated by WCPP Client Utility 
                Dim public_key_base64 As String = context.Request("wcp_pub_key_base64")
                Dim public_key_signature_base64 As String = context.Request("wcp_pub_key_signature_base64")

                If (String.IsNullOrEmpty(public_key_base64)) Then

                    context.Response.StatusCode = 400
                    context.Response.ContentType = "text/plain"
                    context.Response.Write("No public key provided.")

                Else

                    'ALL the test files are protected with the same password for demo purposes 
                    'This password will be encrypted And stored in file metadata
                    Dim plainTextPassword As String = "ABC123"

                    'create print file with password protection
                    Dim file As PrintFile
                    If (fileType = "PDF") Then
                        file = New PrintFilePDF(context.Server.MapPath(filePath), fileName)
                        DirectCast(file, PrintFilePDF).Password = plainTextPassword
                        'DirectCast(file, PrintFilePDF).PrintRotation = PrintRotation.None
                        'DirectCast(file, PrintFilePDF).PagesRange = "1,2,3,10-15"
                        'DirectCast(file, PrintFilePDF).PrintAnnotations = True
                        'DirectCast(file, PrintFilePDF).PrintAsGrayscale = True
                        'DirectCast(file, PrintFilePDF).PrintInReverseOrder = True
                    ElseIf (fileType = "DOC") Then
                        file = New PrintFileDOC(context.Server.MapPath(filePath), fileName)
                        DirectCast(file, PrintFileDOC).Password = plainTextPassword
                        'DirectCast(file, PrintFileDOC).PagesRange = "1,2,3,10-15"
                        'DirectCast(file, PrintFileDOC).PrintInReverseOrder = True
                    Else
                        file = New PrintFileXLS(context.Server.MapPath(filePath), fileName)
                        DirectCast(file, PrintFileXLS).Password = plainTextPassword
                        'DirectCast(file, PrintFileXLS).PagesFrom = 1
                        'DirectCast(file, PrintFileXLS).PagesTo = 5
                    End If

                    'create an encryption metadata to set to the PrintFile
                    Dim encMetadata = New EncryptMetadata(public_key_base64, public_key_signature_base64)

                    'set encyption metadata to PrintFile to ENCRYPT the Password to unlock the file
                    file.EncryptMetadata = encMetadata

                    'create ClientPrintJob for printing encrypted file
                    Dim cpj As New ClientPrintJob()
                    cpj.PrintFile = file
                    If useDefaultPrinter OrElse printerName = "null" Then
                        cpj.ClientPrinter = New DefaultPrinter()
                    Else
                        cpj.ClientPrinter = New InstalledPrinter(printerName)
                    End If

                    context.Response.ContentType = "application/octet-stream"
                    'set the ClientPrintJob content
                    context.Response.BinaryWrite(cpj.GetContent())

                    context.Response.End()

                End If
            End If
        Else
            context.Response.StatusCode = 400
            context.Response.ContentType = "text/plain"
            context.Response.Write(context.Request.Url.Query())
            context.Response.End()
        End If

    End Sub


    Public ReadOnly Property IsReusable() As Boolean Implements IHttpHandler.IsReusable
        Get
            Return False
        End Get
    End Property

End Class