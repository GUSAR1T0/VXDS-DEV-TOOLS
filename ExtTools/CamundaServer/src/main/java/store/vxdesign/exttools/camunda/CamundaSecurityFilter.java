package store.vxdesign.exttools.camunda;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.boot.autoconfigure.condition.ConditionalOnProperty;
import org.springframework.boot.web.servlet.FilterRegistrationBean;
import org.springframework.context.annotation.Bean;
import org.springframework.context.annotation.Configuration;
import org.springframework.core.env.Environment;

import javax.servlet.Filter;
import javax.servlet.http.HttpServletRequest;
import javax.servlet.http.HttpServletResponse;
import java.util.Base64;

@Configuration
public class CamundaSecurityFilter {

    private final Environment env;

    @Autowired
    public CamundaSecurityFilter(Environment env) {
        this.env = env;
    }

    @Bean
    @ConditionalOnProperty(name = "camunda.bpm.rest-auth.required")
    public FilterRegistrationBean<Filter> registerAuthenticationProvider() {
        var registration = new FilterRegistrationBean<>();
        registration.setName("camunda-auth");
        registration.setFilter(createAuthenticationProvider());
        registration.addInitParameter("authentication-provider", "org.camunda.bpm.engine.rest.security.auth.impl.HttpBasicAuthenticationProvider");
        registration.addUrlPatterns("/rest/*");
        return registration;
    }

    @Bean
    @ConditionalOnProperty(name = "camunda.bpm.rest-auth.required")
    public Filter createAuthenticationProvider() {
        return (servletRequest, servletResponse, filterChain) -> {
            var authHeader = ((HttpServletRequest) servletRequest).getHeader("Authorization");
            if (authHeader != null && authHeader.equals(getBasicAuth()))
                filterChain.doFilter(servletRequest, servletResponse);
            else {
                HttpServletResponse response = (HttpServletResponse) servletResponse;
                response.setHeader("WWW-Authenticate", "Basic realm=\"Protected\"");
                response.sendError(401, "Authentication is required");
            }
        };
    }

    private String getBasicAuth() {
        var user = env.getProperty("camunda.bpm.admin-user.id");
        var pass = env.getProperty("camunda.bpm.admin-user.password");
        return "Basic " + Base64.getEncoder().encodeToString((user + ':' + pass).getBytes());
    }
}
