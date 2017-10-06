package org.cqrs101.shared.users;

import java.util.UUID;

public interface UsersService {

    UserDto getUser(UUID id);
}
