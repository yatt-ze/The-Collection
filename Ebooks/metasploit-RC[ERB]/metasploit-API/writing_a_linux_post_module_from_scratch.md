



# INTRO
                                 'writing a linux post module from scratch'
                Metasploit modules are written in ruby. Even if you don’t know a lot about ruby,
             you should still be able to write a metasploit linux post module based on this tutorial!

![msf-auxiliarys](http://i.cubeupload.com/m77AgR.png)

<br /><br />  

# MODULE OBJECTIVES
                Gather system information after successfully exploitation (post-exploitation).
                For that we are going to load the msf/core/post/common module to use the msf
                API 'cmd_exec()' to execute bash commands on remote system, display outputs,
                and store outputs as logfile (store_loot).

<br /><br />  

# MODULE DEVELOP METHODOLOGY

               1 - Research stage: For bash commands that can be used to dump target system information
               2 - Writing the metasploit template: libraries, class name, mixins, description (def initialize)
               3 - The exploit code: Basic explaination of ruby syntax + metasploit APIs (def run)
               4 - Port module to msf database: Copy module to the rigth location and reload msfdb


<br /><br /><br /><br />

---
# WRITING A MSF POST MODULE (step by step)
---

# 1 - The research stage:

      At this stage (research) i have used online articles from different websites to store
      possible interesting bash commands that can be used in gathering target system information.
      The follow screenshot shows some of the bash commands that i have stored in a text file.

![msf-auxiliarys](http://i.cubeupload.com/YK7B6f.png)


<br /><br /><br /><br />


# 2 - Writing the metasploit template

      The metasploit template its divided in to 3 main funtions.
      1 - the libraries, class name, mixins: that contains module requires/imports.
      2 - the 'def initialize()' funtion: that contains module description/settings.
      3 - the 'def run()' funtion: that contain the script logic (the exploit code).
      "At this stage we are going to focus in the 'libraries' and 'def initialize()' funtions".

<br /><br />

### The MSF libraries
![msf-auxiliarys](http://i.cubeupload.com/EZbnFy.png)<br />
**rex** the basic library for most tasks: Handles sockets, protocols, text transformations, SSL, SMB, HTTP, XOR, Base64, etc.

**msf/core** will include all the functionalities from the core library. the framework’s core library is the low-level<br />
interface that provides the required functionality for interacting with exploit modules, sessions, plugins, etc.<br />
`HINT: This line alone gives us access to over 6,000+ different functions (API calls).`<br />

**msf/core/post/common** allow us to use the API cmd_exec() to execute bash commands on remote system (linux).<br />
cmd_exec(): http://rapid7.github.io/metasploit-framework/api/Msf/Post/Common.html#cmd_exec-instance_method

<br /><br />

### The Module class name and rank
![msf-auxiliarys](http://i.cubeupload.com/ZcWceG.png)
we begin defining the **class** as MetasploitModule and inherit from **Msf::Post** mixin. Metasploit post modules are special<br />
in that they aren’t necessarily exploits that feature a payload. Instead, they can be considered as **reconnaissance tools**.<br />
This includes port scanners, fuzzers, service fingerprinters, enumeration, information gathering, post-exploitation, etc.

**Rank** = Every module has been assigned a rank based on its potential impact to the target system.<br />
Metasploit ranking system: https://github.com/rapid7/metasploit-framework/wiki/Exploit-Ranking

<br /><br />

### The Msf::Post Mixin
![msf-auxiliarys](http://i.cubeupload.com/gvgw2G.png)
One of the first things that is done is the implementaion of the **Msf::Post** mixin.<br />
When you create a post module with this mixin, a lot of other mixins are also already included.<br />

      HINT: Mixins are a handy mechanism in Ruby language to include functionality into a module.

Msf::Post::File http://rapid7.github.io/metasploit-framework/api/Msf/Post/File.html<br />
Msf::Post::Linux::Priv http://rapid7.github.io/metasploit-framework/api/Msf/Post/Linux/Priv.html<br />
Msf::Post::Linux::System: http://rapid7.github.io/metasploit-framework/api/Msf/Post/Linux/System.html<br />

<br /><br />

### The 'def initialize()' function
Here we need to define some information about the post module, such as:<br />
Module name, description, module author, version, platform, target architecture, DefaultOptions, etc.<br />
![msf-auxiliarys](http://i.cubeupload.com/eHlLPT.png)

Here we can adicionaly config module's default settings using the **DefaultOptions** method
![msf-auxiliarys](http://i.cubeupload.com/4H3A1x.png)


<br /><br />

### The register_options method (show options)
![msf-auxiliarys](http://i.cubeupload.com/qEoaAE.png)
This method adds options that the user can specify before running the module.<br />
The **OptString.new()** API accepts string values (text-numbers-symbols) to be inputed manually by user<br />

**HINT**: in **DefaultOptions** method we allready have defined the module to run againts session 1 by default.<br />
But users can still define (manually) a different session number to run the module againts, example: set SESSION 3  

<br /><br />

### The register_advanced_options method (show advanced options)
![msf-auxiliarys](http://i.cubeupload.com/TSfW5w.png)
This method adds advanced options that the user can specify before running the module.<br />
The **OptBool.new()** API accepts bollean values (1 or 0 - true or false) to be inputed manually by user<br />
The **OptString.new()** API accepts string values (text-numbers-symbols) to be inputed manually by user<br />

<br /><br />

### Close the 'def initialize()' funtion
![msf-auxiliarys](http://i.cubeupload.com/ACpzcT.png)<br />
**HINT**: Funtions in ruby start with **def** (definition) follow by the method name, The method body is enclosed<br />
by this definition on the top and the word **end** (It tells Ruby that we’re done defining the method).


<br /><br /><br /><br />

# 3 - The 'exploit code' (def run)

      The 'def run()' funtion will contain all the 'exploit code' to be executed againts target session.
      It contains the module 'banner' the 'target compatibility checks' and the actual 'exploit code'.

      HINT: loading 'require msf/core/post/common' msf library in the beggining of this post-module,
      allows us to use most of the msf APIs in post-exploitation develop (eg. client.sys.config.getuid).
<br /><br />

### writing the module 'banner'
![msf-auxiliarys](http://i.cubeupload.com/HjxmZp.png)<br />
The **session = client** API tells msf that **session** variable holds the **client** meterpreter communications channel.<br />
The **print_line()** API allow us to write msgs on screen (terminal) and its used to build the module 'banner' in this case.<br />

<br /><br />

### writing the 'target compatibility checks' funtions
![msf-auxiliarys](http://i.cubeupload.com/7OHAEL.png)<br />
The line 12 uses meterpreter **sysinfo['OS']** API to check if target system its a **linux** distro<br />
The line 13 **print_error()** prints a error msg on screen, if none of the strings are returned: **Linux** or **linux**<br />
The line 14 **return nil** will exit module execution, if none of the above strings are returned<br />
The line 15 **end** will close the actual funtion, and resumes script execution.<br />

<br /><br />

![msf-auxiliarys](http://i.cubeupload.com/dDDnow.png)<br />
The line 19 will use **getuid** meterpreter API call, to check if we are running in an higth integrity context (root)<br />
`HINT: then it stores the returned string into a local variable named 'target_uid' for later use.`<br /><br />
The line 20 reads the **target_uid** local variable, and searchs for **uid=0** or **root** strings present<br />
`HINT: that reveal us that we are running under a privileged session (root privileges).`<br /><br />
The line 21 **print_error()** prints a error msg on screen, if none of the strings are returned: **uid=0** or **root**<br />
The line 22 **return nil** will exit module execution, if none of the above strings are returned<br />
The line 23 **end** will close the actual funtion, and resumes script execution.<br />

<br /><br />

![msf-auxiliarys](http://i.cubeupload.com/gZOBT6.png)<br />
The line 28 uses **sysinfo** meterpreter API call, to check if we are running in a meterpreter session<br />

      the sysinfo.nil? API checks if the sysinfo command returned, contains a empty string.
      "the non-return of sysinfo command reveals that we are not on a meterpreter session!"

The line 29 **print_status()** prints a msg on screen, if we are running in a meterpreter session.<br />
`HINT: If a meterpreter session its found, then script execution will jump to the end of this funtion`<br /><br />
The line 30 **else** will execute the 'non-meterpreter session' NOT found funtion<br />
The line 31 **print_error()** prints a error msg on screen, if we are NOT running in a meterpreter session.<br />
The line 32 **return nil** will exit module execution, if we are NOT running in a meterpreter session.<br />
The line 33 **end** will close the actual funtion, and resumes script execution.<br />

<br /><br />

### writing the 'exploit code' (target dump funtion)
![msf-auxiliarys](http://i.cubeupload.com/QRvSWa.png)<br />
The line 39 cleans the contents of **data_dump** local variable to be able to accept new data inputs.<br />
The line 40 **print_status()** prints a msg on screen for users to know that the module its working<br />
The line 41 **Rex::sleep(0.5)** makes a pause in script execution for half a second<br />

<br /><br />

#### executing remote bash commands
![msf-auxiliarys](http://i.cubeupload.com/7cXJmO.png)<br />
From line 43 to 46 we are executing bash commands remotelly, and store the results inside local variables<br />
`HINT: 'distro_uname' local variable will contain the results of cmd_exec("uname -a") bash command`<br />

<br /><br />

This next funtion will store the contents of previous scans (stored inside local variables)<br />
into **data_dump** local variable to be able to write the logfile (my way), and display outputs on screen.<br />
![msf-auxiliarys](http://i.cubeupload.com/Axodxo.png)<br />
The line 51 **data_dump << "\n\n"** will append 2 empty lines into **data_dump** local variable.<br />
The line 58 **data_dump << hardware_info** will append the contents of **hardware_info** inside **data_dump**<br />

<br /><br />

### writing the 'agressive_dump' advanced option funtion
`HINT: In ruby every IF funtion must be closed by the END statement.`<br />
![msf-auxiliarys](http://i.cubeupload.com/dnwV7r.png)<br />
The line 74 checks whats the settings of 'AGRESSIVE_DUMP' advanced option (if its **true** then it will run the funtion)<br />
The line 75 **print_status()** prints a msg on screen for users to know that the module its working<br />
The line 76 **Rex::sleep(0.5)** makes a pause in script execution for half a second<br />

<br /><br />

#### executing remote bash commands
![msf-auxiliarys](http://i.cubeupload.com/ByQn9d.png)
From line 78 to 81 we are executing bash commands remotelly, and store the results inside local variables<br />
`HINT: 'cron_tasks' local variable will contain the results of cmd_exec("ls -la /etc/cron*") bash command`<br />

<br /><br />

This next funtion will store the contents of previous scans (stored inside local variables)<br />
into **data_dump** local variable to be able to write the logfile, and display outputs on screen.<br />
![msf-auxiliarys](http://i.cubeupload.com/BjByPP.png)
The line 102 **end** will close the actual funtion (agressive_dump), and resumes script execution.<br />
`HINT: In ruby every IF funtion must be closed by the END statement.`

<br /><br />

#### Display scans results on screen
The next funtion will display the contents of **data_dump** local variable (remote scan reports) on screen<br />
![msf-auxiliarys](http://i.cubeupload.com/bZzIoc.png)
The line 109 **print_status()** prints a msg on screen for users to know that the module its working<br />
The line 112 **print_line(data_dump)** will print on screen the contents of **data_dump** (remote scan reports)<br />

<br /><br />

#### Store stolen data from session as a logfile
This next funtion will store the contents (logfile) of **data_dump** into ~/msf4/loot folder<br />
![msf-auxiliarys](http://i.cubeupload.com/r4xi9j.png)
The line 120 checks whats the settings of **'STORE_LOOT'** advanced option (if its **true** then it will run the funtion)<br />
The line 121 **print_status()** prints a msg on screen for users to know that the module its working<br />
The line 122 **store_loot()** its a special msf API that allow us to store some data stolen from a session as a file<br />
http://www.rubydoc.info/github/rapid7/metasploit-framework/Msf/Auxiliary/Report#store_loot-instance_method

<br /><br />

This next funtion its self-explanatory :D **(the end of the msf module)**.
![msf-auxiliarys](http://i.cubeupload.com/KZAwaY.png)

<br /><br /><br /><br />

# 4 - Port module to metasploit database

      At this stage we need to port our post-module to the rigth location inside metasploit
      directory structure, reload the database with the new module and load/run the module.

      HINT: reloading our module to msfdb will reveal us if any syntax error as commited.
      loading/executing the module will also reveal us if any sintax errors as commited.

<br /><br />

#### Download linux_hostrecon V1.5 (latest review)
`wget https://github.com/r00t-3xp10it/msf-auxiliarys/blob/master/linux/linux_hostrecon.rb`
#### port the module to msf directory
`cp linux_hostrecon.rb /usr/share/metasploit-framework/modules/post/linux/gather/linux_hostrecon.rb`
#### start postgresql service
`service postgresql start`
#### rebuild msfdb database (database.yml)
`msfdb reinit`
#### reload all modules into database
`msfconsole -q -x 'db_status; reload_all'`
#### search for module
`msf > search linux_hostrecon`
#### load module
`msf > use post/linux/gather/linux_hostrecon`
#### show all module options
`msf post(linux_hostrecon) > show advanced options`
#### set module options
`msf post(linux_hostrecon) > set SINGLE_COMMAND netstat -ano`
#### execute module (run)
`msf post(linux_hostrecon) > exploit`
#### unset all module options
`msf post(linux_hostrecon) > unset all`

<br /><br />

## Final Notes

      This module requires a session allready open to interact with it 
      This module requires a meterpreter session to run
      This module requires a linux distro as target
      This module requires root privileges to execute tasks
      This module stores dump logfiles into ~msf4/loot folder
      This module enables users to run options indevidually or all at once
      This module enables users to input a single bash command to be executed

      HINT: the single_command option accepts [ ; | && ] to stack bash commands.
      EXAMPLE: ls -a; cat /etc/passwd | cut -d ":" -f1 && find /var/log -type f -perm -4

<br /><br />

### Automating the post-module
![msf-auxiliarys](http://i.cubeupload.com/us441y.png)
If we wish to run all module options at once, then simple edit the module before port it to metasploit, add the above lines<br />
on it, save it and use it. the above lines will config that options to run auto at module execution (exploit or run). 

<br /><br />

### Screenshots of the post-module running
![msf-auxiliarys](http://i.cubeupload.com/SQ0eDb.png)
![msf-auxiliarys](http://i.cubeupload.com/WbYvm1.png)
![msf-auxiliarys](http://i.cubeupload.com/JYIF0C.png)
![msf-auxiliarys](http://i.cubeupload.com/tKpRcj.png)



<br /><br />

#### This article template (v1.0):
https://pastebin.com/7DZm7W4U

#### linux_hostrecon.rb (v1.5) sourcecode:
https://github.com/r00t-3xp10it/msf-auxiliarys/blob/master/linux/linux_hostrecon.rb

#### linux_hostrecon.rb (v1.3) video tutorial:
https://www.youtube.com/watch?v=t2zTSZc0gAc

<br /><br />

## REFERENCES
http://rapid7.github.io/metasploit-framework/api/<br />
https://www.offensive-security.com/metasploit-unleashed/building-module/<br />
https://www.offensive-security.com/metasploit-unleashed/creating-auxiliary-module/<br />
https://www.cyberciti.biz/tips/linux-command-to-gathers-up-information-about-a-linux-system.html<br />
https://github.com/rapid7/metasploit-framework/wiki/How-to-get-started-with-writing-a-post-module<br />
https://github.com/r00t-3xp10it/hacking-material-books/blob/master/metasploit-RC[ERB]/metasploit-API/my-API-Cheat-sheet<br />

# Suspicious-Shell-Activity (SSA) RedTeam develop @2017

