﻿@Code
    ViewData("Title") = "Home Page"
End Code

@section styles

<style type="text/css">
    .glyphicon-refresh-animate {
        -animation: spin .7s infinite linear;
        -webkit-animation: spin2 .7s infinite linear;
    }

    @@-webkit-keyframes spin2 {
        from {
            -webkit-transform: rotate(0deg);
        }

        to {
            -webkit-transform: rotate(360deg);
        }
    }

    @@keyframes spin {
        from {
            transform: scale(1) rotate(0deg);
        }

        to {
            transform: scale(1) rotate(360deg);
        }
    }
</style>

End Section


<div id="msgInProgress">
    <div id="mySpinner" style="width:32px;height:32px"></div>
    <br />
    <h3>Detecting WCPP utility at client side...</h3>
    <h3><span class="label label-info"><span class="glyphicon glyphicon-refresh glyphicon-refresh-animate"></span> Please wait a few seconds...</span></h3>
    <br />
</div>
<div id="msgInstallWCPP" style="display:none;">
    <h3>#1 Install WebClientPrint Processor (WCPP)!</h3>
    <p>
        <strong>WCPP is a native app (without any dependencies!)</strong> that handles all print jobs
        generated by the <strong>WebClientPrint for ASP.NET component</strong> at the server side. The WCPP
        is in charge of the whole printing process and can be
        installed on <strong>Windows, Linux, Mac & Raspberry Pi!</strong>
    </p>
    <p>
        <a href="//www.neodynamic.com/downloads/wcpp/" target="_blank" class="btn btn-warning">Download and Install WCPP from Neodynamic website</a><br />
    </p>
    <h3>#2 After installing WCPP...</h3>
    <p>
        <a href="@Url.Action("Samples", "Home")" class="btn btn-info">You can go and test the printing page...</a>
    </p>

</div>


@section scripts

<script type="text/javascript">

            //var wcppPingDelay_ms = 5000;

    var wcppPingTimeout_ms = 60000; //60 sec
    var wcppPingTimeoutStep_ms = 500; //0.5 sec

            function wcppDetectOnSuccess(){
                // WCPP utility is installed at the client side
                // redirect to WebClientPrint sample page

                // get WCPP version
                var wcppVer = arguments[0];
                if(wcppVer.substring(0, 1) == "5")
                    window.location.href = '@Url.Action("samples", "home")';
                else //force to install WCPP v5.0
                    wcppDetectOnFailure();
            }

            function wcppDetectOnFailure() {
                // It seems WCPP is not installed at the client side
                // ask the user to install it
                $('#msgInProgress').hide();
                $('#msgInstallWCPP').show();
            }

</script>


@* WCPP detection script generated by HomeController *@

@Html.Raw(ViewData("WCPPDetectionScript"))


    }

End Section