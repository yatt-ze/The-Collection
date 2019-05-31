 
{include file="header.tpl"}
{include file="nav.tpl"}


<div class="col-md-11 col-lg-11">
        <div class="container">

            <div class="row">
                <div class="col-md-6 col-lg-6">
                    <div id="vmap" style="width: 600px; height:400px;"></div>
                </div>
                <div class="col-md-6 col-lg-6">
                     <!--  <h2>Some TODO</h2> -->
                </div>
            </div>




            <table class="table table-bordered">
                <thead>
                    <tr>
                    <th>Country</th>
                    <th>Computrername</th>
                    <th>Opering System</th>
                    <th>Status</th>
                    </tr>
                </thead>
                <tbody>
                    {foreach from=$tasks item=info}
                        <tr>
                            <td>{$info.country}</td>
                            <td>{$info.computrername}</td>
                            <td>{$info.operingsystem}</td>
                            <td>{$info.status}</td>
                        </tr>
                    {/foreach}
                </tbody>
            </table>
        </div>
</div>
        
{include file="footer.tpl"}


<script>
    generateWordMap({$worldmap});
</script>
