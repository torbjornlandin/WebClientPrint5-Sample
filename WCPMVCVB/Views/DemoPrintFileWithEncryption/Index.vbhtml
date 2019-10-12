﻿@Code
    ViewData("Title") = "Print Known File Formats with Encryption"
End Code

<div class="container">
    <div class="row">

        <h3><a href="/home/samples" class="btn btn-md btn-danger"><i class="fa fa-chevron-left"></i></a>&nbsp;Print Encrypted Files without displaying any Print dialog! <small>(if needed)</small></h3>
        <p>
            With <strong>WebClientPrint for ASP.NET</strong> solution you can <strong>print encrypted files in PDF, TXT, JPG/JPEG &amp; PNG formats</strong> right to any installed printer at the client side.
        </p>

        <div class="alert alert-info">
            <strong>WebClientPrint uses RSA</strong> asymetric encryption with a <strong>dynamic public key generated by WCPP utility</strong> for each printing request to encrypt the <strong>AES-256</strong> params. The file to print is encrypted by using <strong>AES-256</strong> symetric encryption in the server side and stored at the client machine disk. At printing time, WCPP will decrypt the file to print in system memory.
        </div>

        <div class="well">
            <p>
                The following are pre-selected files to test WebClientPrint File Printing with Encryption feature enabled.
            </p>
            <div class="row">
                <div class="col-md-4 col-md-offset-2">
                    <hr />
                    <div class="checkbox">
                        <label>
                            <input type="checkbox" id="useDefaultPrinter" />
                            <strong>Print to Default printer</strong> or...
                        </label>
                    </div>
                    <div id="loadPrinters">
                        Click to load and select one of the installed printers!
                        <br />
                        <a onclick="javascript:jsWebClientPrint.getPrinters();" class="btn btn-success">Load installed printers...</a>
                        <br />
                        <br />
                    </div>
                    <div id="installedPrinters" style="visibility: hidden">
                        <label for="installedPrinterName">Select an installed Printer:</label>
                        <select name="installedPrinterName" id="installedPrinterName" class="form-control"></select>
                    </div>
                    <script type="text/javascript">
                            //var wcppGetPrintersDelay_ms = 0;
                            var wcppGetPrintersTimeout_ms = 60000; //60 sec
                            var wcppGetPrintersTimeoutStep_ms = 500; //0.5 sec
                            function wcpGetPrintersOnSuccess() {
                                // Display client installed printers
                                if (arguments[0].length > 0) {
                                    var p = arguments[0].split("|");
                                    var options = '';
                                    for (var i = 0; i < p.length; i++) {
                                        options += '<option>' + p[i] + '</option>';
                                    }
                                    $('#installedPrinters').css('visibility', 'visible');
                                    $('#installedPrinterName').html(options);
                                    $('#installedPrinterName').focus();
                                    $('#loadPrinters').hide();
                                } else {
                                    alert("No printers are installed in your system.");
                                }
                            }
                            function wcpGetPrintersOnFailure() {
                                // Do something if printers cannot be got from the client
                                alert("No printers are installed in your system.");
                            }
                    </script>
                </div>
                <div class="col-md-4">
                    <hr />
                    <div id="fileToPrint">
                        <label for="ddlFileType">Select a sample File to print:</label>
                        <select id="ddlFileType" class="form-control">
                            <option>PDF</option>
                            <option>TXT</option>
                            <option>JPG</option>
                            <option>PNG</option>
                        </select>
                        <br />
                        <a class="btn btn-success btn-lg" onclick="javascript:jsWebClientPrint.print('encrypt=true' + '&useDefaultPrinter=' + $('#useDefaultPrinter').attr('checked') + '&printerName=' + encodeURIComponent($('#installedPrinterName').val()) + '&filetype=' + $('#ddlFileType').val());">Print File...</a>
                    </div>
                </div>
            </div>

        </div>


    </div>
</div>

@section scripts

    @* Register the WebClientPrint script code generated by DemoPrintFileWithEncryptionController. *@

    @Html.Raw(ViewData("WCPScript"))


end section



