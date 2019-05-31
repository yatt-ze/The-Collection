 
{include file="header.tpl"}
{include file="nav.tpl"}


<!-- Modal -->
<div class="modal fade" id="createnewUser" tabindex="-1" role="dialog" aria-labelledby="exampleModalLabel" aria-hidden="true">
    <div class="modal-dialog" role="document">
        <div class="modal-content">
            <div class="modal-header">
                <h5 class="modal-title" id="exampleModalLabel">Create new User</h5>
                <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                    <span aria-hidden="true">&times;</span>
                </button>
            </div>
            <form method="POST">
            <div class="modal-body">

                   <div class="form-group">
                       <label for="createuser_password">Username</label>
                       <input type="text" class="form-control" name="createuser_username"  id="createuser_password" placeholder="Username" required>

                   </div>
                   <div class="form-group">
                       <label for="createuser_password">Passowrd</label>
                       <input type="password" class="form-control" name="createuser_password"  id="createuser_password"  placeholder="Password" required>

                   </div>


            </div>
            <div class="modal-footer">
                <button type="button" class="btn btn-secondary" data-dismiss="modal">Close</button>
                <button type="submit" class="btn btn-primary">Save changes</button>
            </div>
            </form>
        </div>
    </div>
</div>


<div class="col-md-11 col-lg-11">
        <div class="container">

            <ul class="nav nav-tabs" role="tablist">
                <li class="nav-item">
                  <a class="nav-link active" href="#users" role="tab" data-toggle="tab">Users</a>
                </li>
                <li class="nav-item">
                  <a class="nav-link" href="#globalsettings" role="tab" data-toggle="tab">Global Settings</a>
                </li>
                <li class="nav-item">
                  <a class="nav-link" href="#functions" role="tab" data-toggle="tab">Functions</a>
                </li>
                <li class="nav-item">
                    <a class="nav-link" href="#update" role="tab" data-toggle="tab">Update</a>
                </li>
            </ul>
              
              <!-- Tab panes -->
              <div class="tab-content">
                <div role="tabpanel" class="tab-pane active" id="users">

                    <table class="table">
                        <thead>
                          <tr>
                            <th>Username</th>
                            <th>Edit
                                <button type="button" class="btn btn-primary createnewUser" data-toggle="modal" data-target="#createnewUser">
                                    Create new User
                                </button>
                            </th>
                          </tr>
                        </thead>
                        <tbody>
              
                          {foreach from=$users item=user}
                          <tr>
                            <td>{$user.username}</td>
                            <td>
                                <form method="POST" id="blockuser-{$user.id}">
                                    <a href="/edituser/{$user.id}"> Edit </a>
                                    <input name="userid" value="{$user.id}" hidden>
                                    {if $user.active == "1"}
                                        <input name="blockuser" value="lock" hidden>
                                        <img width="19" onclick="document.getElementById('blockuser-{$user.id}').submit()" src="{$includeDir}assets/img/unlock.svg" title="User is Active">
                                    {else}
                                        <input name="blockuser" value="unlock" hidden>
                                        <img width="19" onclick="document.getElementById('blockuser-{$user.id}').submit()" src="{$includeDir}assets/img/lock.svg" title="User is Banned">
                                    {/if}
                                </form>
                            </td>
                          </tr>
                          {/foreach}
 
                        </tbody>
                      </table>
          
                </div>
                <div role="tabpanel" class="tab-pane fade" id="globalsettings">
                    <form method="POST">
                        <div class="form-group">
                            <label for="updateinfo">Update Information URL</label>
                            <input type="text" class="form-control" name="updateinfo" value="{$config.check_update_url}" id="updateinfo" aria-describedby="emailHelp" placeholder="Enter your encryption key (From bot config.h)">
                            <small id="updateinfoHelper" class="form-text text-muted">Check New versions from Darkspider.</small>
                        </div>
                        <div class="form-group">
                            <label for="enryptionkey">Enryption Key</label>
                            <input type="text" class="form-control" name="enryptionkey" value="{$config.enryptionkey}" id="enryptionkey" aria-describedby="emailHelp" placeholder="Enter your encryption key (From bot config.h)">
                            <small id="enryptionkeyHelper" class="form-text text-muted">We'll never share your encryption key with anyone else. (This is the XOR Cipher Private Key)</small>
                        </div>
                        <div class="form-group">
                            <label for="useragent">User Agent</label>
                            <input type="text" class="form-control" name="useragent" value="{$config.useragent}" id="useragent" aria-describedby="emailHelp" placeholder="Enter your encryption key (From bot config.h)">
                            <small id="useragentHelper" class="form-text text-muted">The Bot and the Gate need the same HTTP User Agent.</small>
                        </div>
                        <input type="submit" class="btn btn-dark" value="Save">
                    </form>
                </div>
                <div role="tabpanel" class="tab-pane fade" id="update">
                    <div class="updatecenter">
                     <a class="bttn version_check" id="1" style="cursor: pointer;">Check for Updates</a>
                        <div class="loading"></div>
                    </div>
                </div>
                  <div role="tabpanel" class="tab-pane fade" id="functions">
                      <form method="POST">
                          {if $encryptedOUT != ""}
                              <div class="alert alert-success" role="alert">
                                 Add this to your Pastebin: <b><pre>{$encryptedOUT}</pre></b>
                              </div>
                              <a  class="btn btn-dark"  href="/settings">Reload</a>
                          {else}
                              <div class="form-group">
                                  <label for="encrypt">Encrypt & Decrypt XOR Cipher</label>
                                  <input type="text" class="form-control" name="encrypt"  id="encrypt" aria-describedby="emailHelp" placeholder="By Default: http://0.0.0.0/request">
                                  <small id="encryptHelper" class="form-text text-muted">Encrypt your current Server URL before create a Pastebin with it. (0.0.0.0 is your IP)</small>
                              </div>
                              <input type="submit" class="btn btn-dark" value="Encrypt">
                          {/if}
                      </form>
                  </div>
              </div>

        </div>
</div>
        
{include file="footer.tpl"}

<script>


    // function to reorder
    $(document).ready(function(){
        // check users files and update with most recent version
        $(".version_check").on('click',function(e) {
            //$(".loading").show();
            var uid = $(this).attr("id");
            var info = "uid="+uid+"&vcheck=1";
            $.ajax({
                beforeSend: function(){
                    $(".loading").html('<br><img src="loader.gif" width="16" height="16" />');
                },
                type: "POST",
                url: "version_check",
                data: info,
                dataType: "json",
                success: function(data){
                    // clear loading information
                    $(".loading").html("");
                    // check for version verification
                    if(data.version != 0){
                        $(".version_check").remove();
                        $(".loading").html("<p><b>"+data.version+"</b> Version is Released. You are outdated</p>  <a id='1' class='bttn doUpdate'>START UPDATE</a>");


                        $(".doUpdate").on('click',function(e) {
                            //$(".loading").show();
                            var uid = $(this).attr("id");
                            var info = "uid="+uid+"&vcheck=1";
                                    // clear loading information
                                    $(".loading").html("");
                                    // check for version verification
                                    if(data.version != 0){
                                        var uInfo = "uid="+uid+"&vnum="+data.version
                                        $.ajax({
                                            beforeSend: function(){
                                                $(".loading").html('<br><img src="loader.gif" width="16" height="16" />');
                                            },
                                            type: "POST",
                                            url: "doUpdate",
                                            data: uInfo,
                                            dataType: "json",
                                            success: function(data){
                                                // check for version verification
                                                if(data.copy != 0){
                                                    if(data.unzip == 1){
                                                        // clear loading information
                                                        $(".version_check").html("");
                                                        // successful update
                                                        $(".loading").html("Successful Update!");
                                                    }else{
                                                        // error during update/unzip
                                                        $(".loading").html("<br>Sorry, there was an error with the update.");
                                                    }
                                                }
                                            },
                                            error: function() {
                                                // error
                                                $(".loading").html('<br>There was an error updating your files.');
                                            }
                                        });
                                    }
                        });


                    }else{
                        // user has the latest version already installed
                        $(".version_check").remove();
                        $(".loading").html("You already have the latest version.");
                    }
                },
                error: function() {
                    // error
                    $(".loading").html('<br>There was an error checking your latest version.');
                }
            });
        });


        // check users files and update with most recent version

    });










// wire up shown event
$('a[data-toggle="tab"]').on('shown.bs.tab', function (e) {
    console.log("tab shown...");
    localStorage.setItem('activeTab', $(e.target).attr('href'));
});

// read hash from page load and change tab
var activeTab = localStorage.getItem('activeTab');
if(activeTab){
    $('.nav-tabs a[href="' + activeTab + '"]').tab('show');
}
</script>