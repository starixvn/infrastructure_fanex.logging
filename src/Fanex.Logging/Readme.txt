Add config:
<configSections>        
        <section name="Fanex.Logging" type="Fanex.Logging.Configuration.LoggingSettings, Fanex.Logging"/>        
</configSections>
<Fanex.Logging msmqPath="FormatName:DIRECT=tcp:[ip]\private$\entlib" defaultCategory="LiveCasino"/>
or 
<Fanex.Logging msmqPath="FormatName:DIRECT=OS:[machine-name]\private$\entlib" defaultCategory="LiveCasino"/>
