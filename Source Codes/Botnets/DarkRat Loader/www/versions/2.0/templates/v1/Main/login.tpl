{include file="header.tpl"}


<div class="login">

    <div class='login-table'>
        <div class='login-cell'>
            <form id="login" class="login-form" method="POST">
                <label>Username:</label>
                <input type="text" name="userid" size="18" maxlength="18" />
                <br />
                <label>Password :</label>
                <input type="password" name="pswrd" size="18" maxlength="18" />
                <br />
                <label><input type="checkbox" name="save_login" value="1"> You hate logging in? <br> Store up this browser to save the login</label><br>
                <br />
                <a  onclick="document.getElementById('login').submit()" class="bttn">Continue</a>
            </form>
        </div>
    </div>
</div>
