package org.cqrs101;

import org.springframework.context.annotation.Configuration;
import org.springframework.context.annotation.PropertySource;

@Configuration
@PropertySource("classpath:*.properties")
public class AppConfig {
}
