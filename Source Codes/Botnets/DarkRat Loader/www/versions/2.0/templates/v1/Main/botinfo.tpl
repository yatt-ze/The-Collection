


{include file="header.tpl"}
{include file="nav.tpl"}


<div class="col-md-11 col-lg-11">
    <div class="container">

        <div class="card" style="">
            <div class="card-body">
                <h5 class="card-title">Bot Informations</h5>

                 <table class="botinfo">
                     <tr>
                         <td>Hardware UUID</td>
                         <td>{$botinfo.hwid}</td>
                     </tr>
                     <tr>
                         <td>IP Address</td>
                         <td>{$botinfo.ip}</td>
                     </tr>
                     <tr>
                         <td>Computer Name</td>
                         <td>{$botinfo.computrername}</td>
                     </tr>
                     <tr>
                         <td>Antivirus</td>
                         <td>{$botinfo.antivirus}</td>
                     </tr>
                     <tr>
                         <td>Last Seen</td>
                         <td>{$botinfo.lastresponse}</td>
                     </tr>
                     <tr>
                         <td>Install Date</td>
                         <td>{$botinfo.install_date}</td>
                     </tr>
                     <tr>
                         <td>Opering System</td>
                         <td>{$botinfo.operingsystem}</td>
                     </tr>
                     <tr>
                         <td>Country Name</td>
                         <td><img class="flag" src="{$includeDir}assets/img/flags/{$botinfo.country|lower}.png"> |  {$botinfo.country}  |  {$botinfo.countryName}</td>
                     </tr>
                     <tr>
                         <td>.Net2 Installed</td>
                         <td>
                             {if $botinfo.netframework2 == "true"}
                                 <img src="{$includeDir}assets/img/checked.svg">
                             {else}
                                 <img src="{$includeDir}assets/img/error.svg">
                             {/if}
                         </td>
                     </tr>
                     <tr>
                         <td>.Net3 Installed</td>
                         <td>
                         {if $botinfo.netframework3 == "true"}
                            <img src="{$includeDir}assets/img/checked.svg">
                         {else}
                             <img src="{$includeDir}assets/img/error.svg">
                         {/if}
                         </td>
                     </tr>
                     <tr>
                         <td>.Net3.5 Installed</td>
                         <td>
                             {if $botinfo.netframework35 == "true"}
                                 <img src="{$includeDir}assets/img/checked.svg">
                             {else}
                                 <img src="{$includeDir}assets/img/error.svg">
                             {/if}
                         </td>
                     </tr>
                     <tr>
                         <td>.Net4 Installed</td>
                         <td>
                             {if $botinfo.netframework4 == "true"}
                                 <img src="{$includeDir}assets/img/checked.svg">
                             {else}
                                 <img src="{$includeDir}assets/img/error.svg">
                             {/if}
                         </td>
                     </tr>
                     <tr>
                         <td>Latitude</td>
                         <td>{$botinfo.latitude}</td>
                     </tr>
                     <tr>
                         <td>Longitude</td>
                         <td>{$botinfo.longitude}</td>
                     </tr>
                     <tr>
                         <td>Bot Version</td>
                         <td>{$botinfo.version}</td>
                     </tr>
                 </table>
                 <hr>

                <a href="/tasks/{$botinfo.id}" class="btn btn-primary">Execute Task on This Only</a>
                <hr>
                <br>
            </div>
        </div>


    </div>
    </div>
</div>

{include file="footer.tpl"}
