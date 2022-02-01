package com.mypurecloud.sdk.v2.guest.connector.ning;

import com.mypurecloud.sdk.v2.guest.connector.ApiClientConnector;
import com.mypurecloud.sdk.v2.guest.connector.ApiClientConnectorProperties;
import com.mypurecloud.sdk.v2.guest.connector.ApiClientConnectorProperty;
import com.mypurecloud.sdk.v2.guest.connector.ApiClientConnectorProvider;
import org.asynchttpclient.AsyncHttpClient;
import org.asynchttpclient.AsyncHttpClientConfig;
import org.asynchttpclient.DefaultAsyncHttpClient;
import org.asynchttpclient.DefaultAsyncHttpClientConfig;
import org.asynchttpclient.proxy.ProxyServerSelector;
import org.asynchttpclient.util.ProxyUtils;

import java.util.Properties;

public class AsyncHttpClientConnectorProvider implements ApiClientConnectorProvider {
    @Override
    public ApiClientConnector create(ApiClientConnectorProperties properties) {
        DefaultAsyncHttpClientConfig.Builder builder = new DefaultAsyncHttpClientConfig.Builder();

        Integer connectionTimeout = properties.getProperty(ApiClientConnectorProperty.CONNECTION_TIMEOUT, Integer.class, null);
        if (connectionTimeout != null && connectionTimeout > 0) {
            builder.setConnectTimeout(connectionTimeout);
            builder.setReadTimeout(connectionTimeout);
            builder.setRequestTimeout(connectionTimeout);
        }

        ProxyServerSelector proxyServerSelector = ProxyUtils.createProxyServerSelector(new Properties());
        builder.setProxyServerSelector(proxyServerSelector);
        builder.setUseProxySelector(true);
        AsyncHttpClientConfig config = builder.build();
        AsyncHttpClient client = new DefaultAsyncHttpClient(config);
        return new AsyncHttpClientConnector(client);
    }
}
