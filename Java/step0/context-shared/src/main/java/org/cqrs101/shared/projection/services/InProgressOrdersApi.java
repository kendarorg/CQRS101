
package org.cqrs101.shared.projection.services;

import java.util.List;
import java.util.UUID;

public interface InProgressOrdersApi {
    InProgressOrderDto getById(UUID id);
    List<InProgressOrderDto> getAll();
}
