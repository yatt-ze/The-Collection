<!DOCTYPE html>
<html>
<head>
<meta name="viewport" content="width=device-width, initial-scale=1">
<style>
body { font-family: Arial;
    background: #27293d;
    color: white;
}

/* Style the tab */
.tab {
  overflow: hidden;
  border: 1px solid #ccc;
  background-color: #f1f1f1;
}

/* Style the buttons inside the tab */
.tab button {
  background-color: inherit;
  float: left;
  border: none;
  outline: none;
  cursor: pointer;
  padding: 14px 16px;
  transition: 0.3s;
  font-size: 17px;
}

/* Change background color of buttons on hover */
.tab button:hover {
  background-color: #ddd;
}

/* Create an active/current tablink class */
.tab button.active {
  background-color: #ccc;
}

/* Style the tab content */
.tabcontent {
  display: none;
  padding: 6px 12px;
  border: 1px solid #ccc;
  border-top: none;
}
</style>
</head>
<body>

<h2>DarkRat Installer V1</h2>
<p>Computer hacking was like a chemical bond holding us all together.</p>

<div class="tab">
  <button class="tablinks" id="defaultOpen" onclick="openTab(event, 'Requirements')">Requirements</button>
  <button class="tablinks" onclick="openTab(event, 'Database')">Database</button>
  <button class="tablinks" onclick="openTab(event, 'Finishing')">Finishing</button>
</div>

<div id="Requirements" class="tabcontent">
 
    {if $return.mysql == "1"}
        MySql Installed
    {else}
        Please Install MYsql
    {/if}
    <hr>



    {foreach from=$return.writable item=dir}
        {$dir} is Writable <br/>
    {/foreach}
    {foreach from=$return.dontwritable item=dir}
         Please make {$dir} Writable <br/>
    {/foreach}

</div>

<div id="Database" class="tabcontent">
  <h3>Create MySql</h3>

  <form method="POST">
    <label>MySQL Root Username</label>
    <input name="mysqlusername">
    <br>
    <label>MySQL Root Password</label>
    <input name="mysqlpassword">
    <hr>
    <i>This Script Creates a Database with a new user the <strong>Root Login will not be saved</strong></i>
    <hr>
    <input value="Install" type="submit">
  </form>



</div>

<div id="Finishing" class="tabcontent">
  <h3>Tokyo</h3>
  <p>Tokyo is the capital of Japan.</p>
</div>

<script>
function openTab(evt, cityName) {
  var i, tabcontent, tablinks;
  tabcontent = document.getElementsByClassName("tabcontent");
  for (i = 0; i < tabcontent.length; i++) {
    tabcontent[i].style.display = "none";
  }
  tablinks = document.getElementsByClassName("tablinks");
  for (i = 0; i < tablinks.length; i++) {
    tablinks[i].className = tablinks[i].className.replace(" active", "");
  }
  document.getElementById(cityName).style.display = "block";
  evt.currentTarget.className += " active";
}
document.getElementById("defaultOpen").click();
</script>
   
</body>
</html> 
