# DNTProfiler
DNTProfiler.EntityFramework.Core is an EntityFramework 6.1+ profiler.

## Usage
* To start using DNTProfiler.EntityFramework.Core package, add the following lines to app.config/web.config file:
<configuration>
  <entityFramework>
    <interceptors>
      <interceptor type="DNTProfiler.EntityFramework.Core.DatabaseLogger, DNTProfiler.EntityFramework.Core">
        <parameters>
          <parameter value="http://localhost:8080" />
          <parameter value="|DataDirectory|\ErrorsLog.Log" />
        </parameters>
      </interceptor>
    </interceptors>
  </entityFramework>
</configuration>

* To disable the DNTProfiler.EntityFramework.Core, just remove or comment out the above settings.
* To view its real-time collected information and reports, you need to download the DNTProfiler application too.
You can download it from here: https://github.com/VahidN/DNTProfiler/releases
