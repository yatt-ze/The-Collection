
{include file="header.tpl"}
{include file="nav.tpl"}

<!--
it's going to be hard but hard doesn't mean impossible
-->

<div class="col-md-11 col-lg-11">
  <div class="container">
      <div class="row">
          <div class="col-md-5 col-lg-5">
              <div id="vmap" style="width: 600px; height:400px;"></div>
          </div>
          <div class="col-md-7 col-lg-7">
              <div class="row">
                  <div class="col-md-6 col-lg-6"><div class="card-stats card">
                          <div class="card-body">
                              <div class="row"><div class="col-4"><div class="info-icon text-center icon-warning"><img width="80" src="{$includeDir}assets/img/workingguy.svg"></div></div>
                                  <div class="col-8"><div class="numbers"><p class="card-category">Online Bots</p><h3 class="card-title">{$onlinebotcount}</h3></div></div>
                              </div>
                          </div>
                     <!--     <div class="card-footer"><hr><div class="stats"><i class="tim-icons icon-refresh-01"></i> Update Now</div></div> -->
                      </div>
                  </div>
                  <div class="col-md-6 col-lg-6"><div class="card-stats card">
                          <div class="card-body">
                              <div class="row"><div class="col-4"><div class="info-icon text-center icon-warning"><img width="80" src="{$includeDir}assets/img/world.svg"></div></div>
                                  <div class="col-8"><div class="numbers"><p class="card-category">Global Bots</p><h3 class="card-title">{$botcount}</h3></div></div>
                              </div>
                          </div>
                          <!--     <div class="card-footer"><hr><div class="stats"><i class="tim-icons icon-refresh-01"></i> Update Now</div></div> -->
                      </div>
                  </div>
                  <div class="col-md-6 col-lg-6"><div class="card-stats card">
                          <div class="card-body">
                              <div class="row"><div class="col-4"><div class="info-icon text-center icon-warning"><img width="80" src="{$includeDir}assets/img/rip.svg"></div></div>
                                  <div class="col-8"><div class="numbers"><p class="card-category">Dead Bots</p><h3 class="card-title">{$deadbotcount}</h3></div></div>
                              </div>
                          </div>
                          <!--     <div class="card-footer"><hr><div class="stats"><i class="tim-icons icon-refresh-01"></i> Update Now</div></div> -->
                      </div>
                  </div>
                  <div class="col-md-6 col-lg-6"><div class="card-stats card">
                          <div class="card-body">
                              <div class="row"><div class="col-4"><div class="info-icon text-center icon-warning"><img width="80" src="{$includeDir}assets/img/calendar.svg"></div></div>
                                  <div class="col-8">
                                      <div class="numbers special">
                                          <div class="row">
                                              <div class="col-8">
                                                  <p class="card-category">Bots seen in last 12 Hours</p>
                                              </div>
                                              <div class="col-4">
                                                  <h3 class="card-title">{$last12hclientscount}</h3>
                                              </div>
                                          </div>
                                          <div class="row">
                                              <div class="col-8">
                                                  <p class="card-category">Bots seen in last 24 Hours</p>
                                              </div>
                                              <div class="col-4">
                                                  <h3 class="card-title">{$lastclientscount}</h3>
                                              </div>
                                          </div>
                                          <div class="row">
                                              <div class="col-8">
                                                  <p class="card-category">Bots seen in last 7 Days</p>
                                              </div>
                                              <div class="col-4">
                                                  <h3 class="card-title">{$last7clientscount}</h3>
                                              </div>
                                          </div>
                                      </div>
                                  </div>
                              </div>
                          </div>
                          <!--     <div class="card-footer"><hr><div class="stats"><i class="tim-icons icon-refresh-01"></i> Update Now</div></div> -->
                      </div>
                  </div>
              </div>
          </div>
      </div>
      <div class="row">
          <table class="table bot_table">
              <thead>
              <tr>
                  <th>Country</th>
                  <th>IP</th>
                  <th>Computername</th>
                  <th>Antivirus</th>
                  <th>Opering System</th>
                  <th>Version</th>
                  <th>Last Seen</th>
                  <th>More Infos</th>
              </tr>
              </thead>
              <tbody>
                {foreach from=$allbots item=bot}
                  <tr>
                    <td class="flag">  <img src="{$includeDir}assets/img/flags/{$bot.country|lower}.png"> {$bot.country}</td>
                    <td> {$bot.ip}</td>
                    <td> {$bot.computrername}</td>
                    <td> {$bot.antivirus}</td>
                    <td class="operingsystem">  <img src="{$includeDir}assets/img/operingsystems/{$bot.operingsystem}.png"> </td>
                    <td> {$bot.version} </td>
                    <td> <span id="lastSeen-{$bot.id}"></span> <script>$("#lastSeen-{$bot.id}").html( timeDifference("{$bot.now}","{$bot.lastresponse}")) </script> </td>
                    <td>
                       <a href="/botinfo/{$bot.id}">More Infos</a>
                    </td>
                  </tr>
                {/foreach}
              </tbody>
          </table>
      </div>
  </div>
</div>


{include file="footer.tpl"}


<script>
        generateWordMap({$worldmap});
</script>
