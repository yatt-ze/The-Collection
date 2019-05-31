 
{include file="header.tpl"}
{include file="nav.tpl"}


<div class="col-md-11 col-lg-11">
        <div class="container">
          <form method="POST">
   
            <div class="form-group">
              <label for="username">Username:</label>
              <input type="text" class="form-control" value="{$user.username}" id="username" disabled>
            </div>

            <div class="form-group">
              <label for="pwd">New Password:</label>
              <input type="password" name ="password" class="form-control" id="pwd">
            </div>
  
            <button type="submit" class="btn btn-primary">Submit</button>
          </form>
       
        </div>
</div>
        
{include file="footer.tpl"}
